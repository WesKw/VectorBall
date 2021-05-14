using Godot;
using System;

public class RotatingPrism : GameObject
{
	[Export]
	float speed = 20f;
	private Vector3 startRot;
	private KinematicBody body;
	
	protected override void SwitchPressed() {}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		body = GetNode<KinematicBody>("CubeBody");
		startRot = RotationDegrees;
	}
	
	public override void _PhysicsProcess(float delta)
	{
		Vector3 rot = RotationDegrees;
		rot.y += delta * speed;
		RotationDegrees = rot;
	}
	
	protected override void Reset()
	{
		RotationDegrees = startRot;
	}
}
