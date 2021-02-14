using Godot;
using System;

public class Player : Spatial
{
	[Export]
	private int lives = 3;
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
		Translation = new Vector3(0, 5.0f, 0);
		cam = GetNode("CameraPivot/Camera") as Camera;
		camPivot = GetNode("CameraPivot") as Spatial;
		camRotation = new Vector3();
	}

	public override void _Process(float delta)
	{
		camTranslation = playerBody.Translation;
		//camTranslation.z += 4.0f;
		//camTranslation.y += 1.5f;
		camPivot.Translation = camTranslation;
		
		float horizCam = Input.GetActionStrength("cam_right") - Input.GetActionStrength("cam_left");
		//float vertCam = Input.GetActionStrength("cam_up") - Input.GetActionStrength("cam_down");
		if(playerBody.LinearVelocity.y > 1)
		{
			if(camRotation.x >= -60.0f)
				camRotation.x -= 5.0f;
		} else {
			if(camRotation.x <= 0)
				camRotation.x += .5f;
		}
		
		//GD.Print("Y Velocity: " + playerBody.LinearVelocity.y.ToString());
		
		camRotation = camPivot.RotationDegrees;
		camRotation.y  += horizCam * camSpeed;
		camPivot.RotationDegrees = camRotation;
	}
}
