using Godot;
using System;

public class Player : Spatial
{
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
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerBody = GetNode("RigidBody") as RigidBody;
		Translation = new Vector3(0, 10.0f, 0);
		cam = GetNode("CameraPivot/Camera") as Camera;
		camPivot = GetNode("CameraPivot") as Spatial;
		camRotation = new Vector3();
	}
	
	public override void _Process(float delta)
	{
		if(inputAllowed)
		{
			float horiz = Input.GetActionStrength("tilt_right") - Input.GetActionStrength("tilt_left");
			float vert = Input.GetActionStrength("tilt_up") - Input.GetActionStrength("tilt_down");
			gravity.x = horiz * maxTilt;
			gravity.y = -1;
			gravity.z = -vert * maxTilt;
			PhysicsServer.AreaSetParam(GetViewport().FindWorld().Space, (PhysicsServer.AreaParameter)1, gravity);
			
			Vector3 playerVel = playerBody.LinearVelocity;
			camPivot.Translation = playerBody.Translation;
		
			float horizCam = Input.GetActionStrength("cam_right") - Input.GetActionStrength("cam_left");
		
			camRotation = camPivot.RotationDegrees;
			camRotation.y  += horizCam * camSpeed;
		
			if(playerBody.LinearVelocity.y < -10.0f)
			{
				camRotation.x = Mathf.Lerp(camRotation.x, -45.0f, .3f);
			} else {
				camRotation.x = Mathf.Lerp(camRotation.x, 0.0f, .3f);
			}
		
			camPivot.RotationDegrees = camRotation;
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
