using Godot;
using System;

public class Player : Spatial
{
	private const float GRAV_SCALE = 4.0f;
	
	//private bool reset = false;
	private bool inputAllowed = true;
	//private bool grounded;
	private int lives = 3;
	private float maxSpeed = .60f;
	private float maxTilt = 31.0f;
	//private float maxVelocity = 120;
	private float camSpeed = 2.0f;
	private Camera cam;
	private Spatial camPivot;
	//private Vector3 camTranslation;
	private Vector3 camRotation;
	private PlayerBody playerBody;
	private Vector3 gravity = new Vector3(0, -1, 0);
	private System.Timers.Timer fallTimer = new System.Timers.Timer(3500);
	
	[Signal]
	public delegate void on_reset();
	
	[Signal]
	public delegate void game_over();
	
	public int Lives
	{
		get { return lives; }
		set { lives = value; }
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Set up timer
		playerBody = GetNode("RigidBody") as PlayerBody;
		playerBody.Translation = new Vector3(0, 5.0f, 0);
		cam = GetNode("CameraPivot/Camera") as Camera;
		camPivot = GetNode("CameraPivot") as Spatial;
		camRotation = new Vector3();
		Connect("on_reset", GetNode<Main>(".."), nameof(on_reset));
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
		inputAllowed = false;
		GD.Print("Player has fallen");
	}
	
	//When player reaches the goal
	public void Goal()
	{
		gravity = new Vector3(0.0f, 1.0f, 0.0f);
		PhysicsServer.AreaSetParam(GetViewport().FindWorld().Space, (PhysicsServer.AreaParameter)1, gravity);
		GD.Print("Goal!");
		inputAllowed = false;
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
			GD.Print(Lives);
			Reset();
		}
	}
}
