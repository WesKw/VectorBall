using Godot;
using System;

public class WorldTilt : WorldEnvironment
{
	float maxTilt;
	Spatial camPivot;
	
	public float MaxTilt
	{
		get { return maxTilt; }
		set { maxTilt = value; }
	}
	
	public Spatial CamPivot
	{
		get { return CamPivot; }
		set { CamPivot = value; }
	}
	
	public override void _Ready()
	{
		
	}
	
	public override void _PhysicsProcess(float delta)
	{
		/*
		Vector3 gravity = Vector3.Zero;
		//get strength of control stick tilt
		float horiz = Input.GetActionStrength("tilt_right") - Input.GetActionStrength("tilt_left");
		float vert = Input.GetActionStrength("tilt_down") - Input.GetActionStrength("tilt_up");
		//adjust gravity vector based on tilt, using basis.z and basis.x of camera to move ball based on the camera
		gravity = ( camPivot.GlobalTransform.basis.z * vert * Mathf.Rad2Deg(maxTilt) )
				 + ( camPivot.GlobalTransform.basis.x * horiz * Mathf.Rad2Deg(maxTilt) );
		gravity.y = BackgroundSkyRotation.y;
		//Update server vector
		BackgroundSkyRotationDegrees = gravity;
		*/
	}
}
