using Godot;
using System;

public class Player : Spatial
{
	private const float GRAV_SCALE = 4.0f;
	
	private bool inputAllowed = true;
	//private bool grounded;
	private int lives = 3;
	private int collected = 0;
	private const float maxSpeed = .60f;
	private const float maxTilt = 31.0f;
	//private float maxVelocity = 120;
	private float camSpeed = 2.0f;
	private ClippedCamera cam;
	private Spatial camPivot;
	//private Vector3 camTranslation;
	private Vector3 camRotation;
	private PlayerBody playerBody;
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
	
	public int Lives
	{
		get { return lives; }
		set { lives = value; }
	}
	
	public Vector3 PlayerBodyTranslation
	{
		get { return playerBody.Translation; }
		set { }
	}
	
	public void BodyLock(bool state)
	{
		playerBody.AxisLockLinearX = state;
		playerBody.AxisLockLinearY = state;
		playerBody.AxisLockLinearZ = state;
		playerBody.AxisLockAngularX = state;
		playerBody.AxisLockAngularY = state;
		playerBody.AxisLockAngularZ = state;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Set up timer
		playerBody = GetNode("RigidBody") as PlayerBody;
		playerBody.Translation = startPos;
		cam = GetNode("CameraPivot/Camera") as ClippedCamera;
		camPivot = GetNode("CameraPivot") as Spatial;
		animator = GetNode("AnimationPlayer") as AnimationPlayer;
		camRotation = new Vector3();
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
		//get strength of control stick tilt
		float horiz = Input.GetActionStrength("tilt_right") - Input.GetActionStrength("tilt_left");
		float vert = Input.GetActionStrength("tilt_down") - Input.GetActionStrength("tilt_up");
		//adjust gravity vector based on tilt, using basis.z and basis.x of camera to move ball based on the camera
		gravity = ( camPivot.GlobalTransform.basis.z * vert*maxSpeed )
				 + ( camPivot.GlobalTransform.basis.x * horiz*maxSpeed );
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
		Vector3 playerVel = playerBody.LinearVelocity;
		camPivot.Translation = playerBody.Translation;
		
		camRotation = camPivot.RotationDegrees;
		//camRotation.x = Mathf.Lerp(camRotation.x, playerVel.y * -vert * maxTilt, .2f);
		camRotation.x = Mathf.Lerp(camRotation.x, -vert * maxTilt, .2f);
		camRotation.y -= horiz * camSpeed;
		//camRotation.y = Mathf.Lerp(camRotation.y, horiz * playerVel, .2f * camSpeed);
		camRotation.z = Mathf.Lerp(camRotation.z, horiz * maxTilt / 2, .2f);
		
		//Moves the camera above the player if the player is falling
		if(playerBody.LinearVelocity.y < -15.0f)
			camRotation.x = Mathf.Lerp(camRotation.x, -45.0f, .3f);
		else
			camRotation.x = Mathf.Lerp(camRotation.x, 0.0f, .3f);
		
		camPivot.RotationDegrees = camRotation;
		//camPivot.RotationDegrees = cam.GlobalTransform.basis.z;
		//cam.LookAt(Vector3.Up, playerBody.Translation);
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
			Lives--;
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
