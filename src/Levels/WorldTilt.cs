using Godot;
using System;

public class WorldTilt : WorldEnvironment
{
	const float TILT_MULTIPLIER = 6.5f;
	static float maxTilt;
	static Spatial camPivot;
	
	public float MaxTilt
	{
		get { return maxTilt; }
		set { maxTilt = value; }
	}
	
	public Spatial CamPivot
	{
		get { return CamPivot; }
		set { camPivot = value; }
	}
	
	public override void _Ready()
	{
		
	}
	
	public override void _PhysicsProcess(float delta)
	{
		/*
		Vector3 tilt = Vector3.Zero;
		//get strength of control stick tilt
		float horiz = Input.GetActionStrength("tilt_right") - Input.GetActionStrength("tilt_left");
		float vert = Input.GetActionStrength("tilt_down") - Input.GetActionStrength("tilt_up");
		//adjust gravity vector based on tilt, using basis.z and basis.x of camera to move ball based on the camera
		tilt = ( camPivot.GlobalTransform.basis.x * -vert * Mathf.Rad2Deg(maxTilt/TILT_MULTIPLIER) )
				 + ( camPivot.GlobalTransform.basis.z * -horiz * Mathf.Rad2Deg(maxTilt/TILT_MULTIPLIER) );
		tilt.y = Environment.BackgroundSkyRotationDegrees.y;
		//Update server vector
		Environment.BackgroundSkyRotationDegrees = tilt;
		*/
	}
}
