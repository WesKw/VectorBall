using Godot;
using System;

public class Player : Spatial
{
	private bool grounded;
	[Export]
	private int lives = 3;
	private float acceleration = 15.0f;
	private float maxVelocity = 120;
	private float camSpeed = 2.0f;
	private Camera cam;
	private Spatial camPivot;
	private Vector3 camTranslation;
	private Vector3 camRotation;
	private RigidBody playerBody;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerBody = GetNode("RigidBody") as RigidBody;
		Translation = new Vector3(0, 10.0f, 0);
		cam = GetNode("CameraPivot/Camera") as Camera;
		camPivot = GetNode("CameraPivot") as Spatial;
		camRotation = new Vector3();
	}
	
	private void AdjustVelocity()
	{
		float horiz = Input.GetActionStrength("tilt_right") - Input.GetActionStrength("tilt_left");
		float vert = Input.GetActionStrength("tilt_up") - Input.GetActionStrength("tilt_down");
		
		Vector3 force = new Vector3(vert * acceleration, horiz * acceleration, 0);
		
		if (vert > 0 || horiz > 0 || vert < 0 || horiz < 0)
		{
			playerBody.AddForce(force, playerBody.Translation);
		}
		else
		{
			
		}
	}
	
	public override void _Process(float delta)
	{
		AdjustVelocity();
		
		Vector3 playerVel = playerBody.LinearVelocity;
		camPivot.Translation = playerBody.Translation;
		
		float horizCam = Input.GetActionStrength("cam_right") - Input.GetActionStrength("cam_left");
		//float vertCam = Input.GetActionStrength("cam_up") - Input.GetActionStrength("cam_down");
		
		camRotation = camPivot.RotationDegrees;
		camRotation.y  += horizCam * camSpeed;
		
		if(playerBody.LinearVelocity.y < -20.0f)
		{
			if(camRotation.x > -45.0f)
				camRotation.x -= 1.0f;
			else
				camRotation.x = -45.0f;
		} else {
			if(camRotation.x < 0.0f)
				camRotation.x += 1.0f;
			else
				camRotation.x = 0.0f;
		}
		
		
		camPivot.RotationDegrees = camRotation;
	}
}
