using Godot;
using System;

public class Elevator : KinematicBody
{
	float time = 0;
	[Export]
	float freq = 1;
	[Export]
	float amp = 14;
	Vector3 velocity = new Vector3(0, 0, 0);
	Vector3 startPos;
	Vector3 currPos;
	Main main;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		startPos = Translation;
		main = GetNodeOrNull("../../..") as Main;
		currPos = startPos;
	}
	
	private float SetYVel(float time)
	{
		float yVel = (float)Math.Sin(time * freq) * amp;
		return yVel;
	}

	public override void _PhysicsProcess(float delta)
	{
		time += delta;
		//velocity.y = SetYVel(time);
		currPos.y = Mathf.Lerp(currPos.y, SetYVel(time), .2f);
		Translation = currPos;
		//MoveAndSlide(velocity, Vector3.Zero, false, 4, .785398f, true);
		
		if(main.Reset)
		{
			time = 0;
			velocity.y = 0;
			Translation = startPos;
		}
	}
}
