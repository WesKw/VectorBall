using Godot;
using System;

public class Player : Spatial
{
	private const float GRAV_SCALE = 4.0f;
	private bool inputAllowed = true;
	private bool grounded;
	[Export]
	private int lives = 3;
	private float maxTilt = 2.0f;
	private float maxVelocity = 120;
	private float camSpeed = 2.0f;
	private Camera cam;
	private Spatial camPivot;
	private Vector3 camTranslation;
	private Vector3 camRotation;
	private RigidBody playerBody;
	private Vector3 gravity = new Vector3(0, -1, 0);
	
	//[Signal]
	//public delegate void on_fallout();
	
	[Signal]
	public delegate void game_over();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerBody = GetNode("RigidBody") as RigidBody;
		Translation = new Vector3(0, 5.0f, 0);
		cam = GetNode("CameraPivot/Camera") as Camera;
		camPivot = GetNode("CameraPivot") as Spatial;
		camRotation = new Vector3();
	}
	
	private void ProcessInput()
	{
		//get strength of control stick tilt
		float horiz = Input.GetActionStrength("tilt_right") - Input.GetActionStrength("tilt_left");
		float vert = Input.GetActionStrength("tilt_down") - Input.GetActionStrength("tilt_up");
		//adjust gravity vector based on tilt, using basis.z and basis.x of camera to move ball based on the camera
		gravity = ( camPivot.GlobalTransform.basis.z * vert ) + ( camPivot.GlobalTransform.basis.x * horiz);
		gravity.y = -1;
		//Update server vector
		PhysicsServer.AreaSetParam(GetViewport().FindWorld().Space, (PhysicsServer.AreaParameter)1, gravity);
	}
	
	private void UpdateCamera()
	{
		Vector3 playerVel = playerBody.LinearVelocity;
		camPivot.Translation = playerBody.Translation;
		
		float horizCam = Input.GetActionStrength("cam_right") - Input.GetActionStrength("cam_left");
		
		camRotation = camPivot.RotationDegrees;
		camRotation.y += horizCam * camSpeed;// * playerVel.x + playerVel.z;
		
		//Moves the camera above the player if the player is falling
		if(playerBody.LinearVelocity.y < -10.0f)
			camRotation.x = Mathf.Lerp(camRotation.x, -45.0f, .3f);
		else
			camRotation.x = Mathf.Lerp(camRotation.x, 0.0f, .3f);
		
		camPivot.RotationDegrees = camRotation;
		
		//cam.LookAt(playerBody.Translation, Vector3.Right);
	}
	
	private void CheckCollisions()
	{
		
	}
	
	public override void _Process(float delta)
	{
		if(inputAllowed)
		{
			ProcessInput();
			UpdateCamera();
			CheckCollisions();
		}
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if(Input.IsActionJustPressed("reset"))
		{
			GetTree().ReloadCurrentScene();
		}
	}
	
	public void on_fallout()
	{
		
	}
}
