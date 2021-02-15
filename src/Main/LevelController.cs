using Godot;
using System;

public class LevelController : Node
{
	private bool inputAllowed = true;
	private float speed = .5f;
	private float maxTilt = 3.5f;
	private KinematicBody currentLevel = null;
	private Vector3 tilt;
	
	public KinematicBody CurrentLevel
	{
		get {return currentLevel;}
		set {currentLevel = value;}
	}
	
	public bool InputAllowed
	{
		get { return inputAllowed; }
		set { inputAllowed = value; }
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tilt = new Vector3();
	}

	public override void _PhysicsProcess(float delta)
	{
		if(inputAllowed)
		{
			float horiz =  Input.GetActionStrength("tilt_left") - Input.GetActionStrength("tilt_right");
			float vert = Input.GetActionStrength("tilt_down") - Input.GetActionStrength("tilt_up");
			
			if(horiz != 0 || vert != 0)
			{
				tilt.x = vert * maxTilt;
				tilt.z = horiz * maxTilt;
					
			} else {
				tilt.x *= .5f;
				tilt.z *= .5f;
			}
			
			currentLevel.RotationDegrees = tilt;
		}
	}
}
