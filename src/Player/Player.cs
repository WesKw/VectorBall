using Godot;
using System;
using System.Linq;

public class Player : Spatial
{
	private const float GRAV_SCALE = 4.0f;
	private bool grounded = true;
	private bool inputAllowed = true;
	//private bool grounded;
	private int lives = 3;
	private int collected = 0;
	private const float maxSpeed = 1.5f;
	private const float maxTilt = 31.0f;
	//private float maxVelocity = 120;
	private float camSpeed = 2.0f;
	private ClippedCamera cam;
	private Spatial camPivot;
	//private Vector3 camTranslation;
	private Vector3 pivotRotation;
	private PlayerBody playerBody;
	private Vector2 camDirection;
	private Vector3 gravity = new Vector3(0, -1, 0);
	private Vector3 startPos = new Vector3(0, 3.0f, 0);
	private System.Timers.Timer fallTimer = new System.Timers.Timer(3500);
	private AnimationPlayer animator;
	
	[Signal]
	public delegate void on_collect(int amount);
	
	[Signal]
	public delegate void on_reset();
	
	[Signal]
	public delegate void game_over();
	
	[Signal]
	public delegate void on_move(float camRot);
	
	[Signal]
	public delegate void update_translation(Vector3 playerBodyTranslation);
	
	public int Lives { get { return lives; } }
	public Vector3 PlayerBodyTranslation { get { return playerBody.Translation; } }
	public float MaxSpeed { get { return maxSpeed; } }
	public Spatial CamPivot { get { return camPivot; } }
	
	public void BodyLock(bool state)
	{
		playerBody.AxisLockLinearX = state;
		playerBody.AxisLockLinearY = state;
		playerBody.AxisLockLinearZ = state;
		playerBody.AxisLockAngularX = state;
		playerBody.AxisLockAngularY = state;
		playerBody.AxisLockAngularZ = state;
	}
	
	private float CheckCollision(Vector3 lastVelocity)
	{
		Godot.Collections.Array colliders = playerBody.GetCollidingBodies();
		//player should be in air
		if(colliders.Count == 0)
			return .2f;
		else
			return 1;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		camDirection = new Vector2(0, 0);
		//Set up timer
		playerBody = GetNode("RigidBody") as PlayerBody;
		playerBody.Translation = startPos;
		cam = GetNode("CameraPivot/Camera") as ClippedCamera;
		camPivot = GetNode("CameraPivot") as Spatial;
		animator = GetNode("AnimationPlayer") as AnimationPlayer;
		pivotRotation = new Vector3();
		Connect("on_reset", GetNode<Main>(".."), nameof(on_reset));
		Connect("on_move", GetNode<GameUI>("../GameUI"), "on_update_player");
		Connect("update_translation", GetNode<GameUI>("../GameUI"), "UpdatePlayerTranslation");
		Connect("on_collect", GetNode<Main>(".."), "UpdateUIScore");
		animator.Connect("animation_finished", GetNode<Main>(".."), "PlayerAnimFinished");
		animator.Connect("animation_finished", this, "IntroFinished");
		Start();
	}
	
	private void ProcessInput()
	{
		Vector3 lastVel = playerBody.LinearVelocity;
		float speedModifier = CheckCollision(lastVel);
		//get strength of control stick tilt
		float vert = Input.GetActionStrength("tilt_down") - Input.GetActionStrength("tilt_up");
		float horiz = Input.GetActionStrength("tilt_right") - Input.GetActionStrength("tilt_left");
		//adjust gravity vector based on tilt, using basis.z and basis.x of camera to move ball based on the camera
		gravity = ( camPivot.GlobalTransform.basis.z * vert*maxSpeed*speedModifier )
				 + ( camPivot.GlobalTransform.basis.x * horiz*maxSpeed*speedModifier );
		gravity.y = -1;
		//Update server vector
		PhysicsServer.AreaSetParam(GetViewport().FindWorld().Space, (PhysicsServer.AreaParameter)1, gravity);
		//Update camera
		EmitSignal(nameof(on_move), camPivot.RotationDegrees.y);
		EmitSignal(nameof(update_translation), playerBody.Translation);
		UpdateCamera(horiz, vert);
	}
	
	private void UpdateCamera(float horiz, float vert)
	{
		camDirection.x = playerBody.LinearVelocity.x / playerBody.LinearVelocity.Length();
		camDirection.y = playerBody.LinearVelocity.z / playerBody.LinearVelocity.Length();
		pivotRotation = camPivot.RotationDegrees;
		float angle = camDirection.AngleTo(Vector2.Up);
		float xTilt = 0f;
		//if(playerBody.LinearVelocity.y < -15.0f)
			//xTilt = Mathf.Lerp(pivotRotation.x, -45.0f, .3f);
		//else
			//xTilt = Mathf.Lerp(pivotRotation.x, 0.0f, .3f);
		float zTilt = 0f;//Mathf.Lerp(pivotRotation.z, horiz * maxTilt / 2, .2f);
		
		//Quat newPivot = new Quat(new Vector3(camPivot.RotationDegrees.x, angle, camPivot.RotationDegrees.z));
		Quat newPivot = new Quat(new Vector3(xTilt, angle, zTilt));
		Quat originalPivot = camPivot.Transform.basis.Quat();
		Quat result = originalPivot.Slerp(newPivot, .025f);
		Basis newBasis = new Basis(result);
		
		Transform cameraTransform = camPivot.Transform;
		cameraTransform.basis = newBasis;
		camPivot.Transform = cameraTransform;
		
		Vector3 playerVel = playerBody.LinearVelocity;
		camPivot.Translation = playerBody.Translation;
		
		//camRotation = camPivot.GlobalTransform.basis.z;
		//pivotRotation.x = Mathf.Lerp(pivotRotation.x, -vert * maxTilt, .2f);
		//pivotRotation.y = camPivot.RotationDegrees.y;
		
		/*
		if(camDirection.x != 0 && camDirection.y != 0)
		{
			pivotRotation.y = Mathf.Lerp(pivotRotation.y, Mathf.Rad2Deg(angle), .05f);
		}
		*/
		
		//camPivot.RotationDegrees = pivotRotation;
		//cam.LookAt(camPivot.Translation, Vector3.Up);
	}
	
	public override void _Process(float delta)
	{
		if(inputAllowed)
		{
			ProcessInput();
		}
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if(Input.IsActionJustPressed("reset") && lives > 0 && inputAllowed) { Reset(); }
	}
	
	public void Reset()
	{
		playerBody.Reset = true;
		inputAllowed = true;
		camPivot.Translation = playerBody.Translation;
		camPivot.RotationDegrees *= 0;
		gravity = new Vector3(0, -1, 0);
		PhysicsServer.AreaSetParam(GetViewport().FindWorld().Space, (PhysicsServer.AreaParameter)1, gravity);
		EmitSignal(nameof(on_reset));
	}
	
	//When player falls off the stage
	public void FallOut()
	{
		//animator.PlaybackSpeed = 1.5f;
		inputAllowed = false;
		//GD.Print("Player has fallen");
	}
	
	//When player reaches the goal
	public void Goal()
	{
		gravity = new Vector3(0.0f, 1.0f, 0.0f);
		PhysicsServer.AreaSetParam(GetViewport().FindWorld().Space, (PhysicsServer.AreaParameter)1, gravity);
		//GD.Print("Goal!");
		inputAllowed = false;
		//animator.PlaybackSpeed = 1;
	}
	
	//Call reset when timer is elapsed
	public void FallTimerElapsed()
	{
		if(lives < 1)
		{
			GetTree().ChangeScene("res://scenes/Title/Title.tscn");
		} else
		{
			lives--;
			//GD.Print(Lives);
			Reset();
		}
	}
	
	public void Start()
	{
		//BodyLock(false);
		playerBody.Show();
		inputAllowed = true;
	}
	
	public void PlayIntro(string anim)
	{
		BodyLock(true);
		playerBody.Hide();
		inputAllowed = false;
		PlayAnimation(anim);
	}
	
	private void PlayAnimation(string anim)
	{
		animator.Play(anim);
	}
	
	private void IntroFinished(string anim)
	{
		if(anim == "CameraSwirl") { Start(); }
	}
	
	private void _on_Area_area_entered(Area area)
	{
		if(area is Collectable)
		{
			collected++;
			if(collected > 100)
			{
				lives++;
				collected = 1;
			}
			EmitSignal(nameof(on_collect), 50);
		}
	}
}
