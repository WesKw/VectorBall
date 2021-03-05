using Godot;
using System;

public class RotatingBridge : KinematicBody
{
	[Export]
	float speed = .05f;
	float time = 0;
	Vector3 rot;
	
	public override void _Ready()
	{
		rot = RotationDegrees;
	}
	
	public override void _PhysicsProcess(float delta)
	{
		time += delta;
		rot.y += (float)Math.Sin(time);
		RotationDegrees = rot;
	}
}
