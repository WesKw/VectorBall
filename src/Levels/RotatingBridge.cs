using Godot;
using System;

public class RotatingBridge : GameObject
{
	[Export]
	float speed = .05f;
	float time = 0;
	KinematicBody bridge;
	
	public override void _Ready()
	{
		bridge = GetChild(0) as KinematicBody;
		rot = bridge.RotationDegrees;
	}
	
	public override void _PhysicsProcess(float delta)
	{
		time += delta;
		rot.y += (float)Math.Sin(time)/2;
		bridge.RotationDegrees = rot;
	}
	
	protected override void Reset()
	{
		GD.Print("Reset bridge");
		rot = Vector3.Zero;
		bridge.RotationDegrees = rot;
	}
}
