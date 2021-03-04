using Godot;
using System;

public class SpaceBackground : Spatial
{
	[Export]
	float speed1 = .09f;
	[Export]
	float speed2 = .05f;
	CSGTorus ring1;
	CSGTorus ring2;
	Vector3 angularRot1;
	Vector3 angularRot2;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ring1 = GetChild(0) as CSGTorus;
		ring2 = GetChild(1) as CSGTorus;
		angularRot1 = ring1.RotationDegrees;
		angularRot2 = ring2.RotationDegrees;
	}

	public override void _PhysicsProcess(float delta)
	{
		angularRot1.x += speed1;
		angularRot1.z += speed1;
		angularRot2.x += speed2;
		angularRot2.z += speed2;
		ring1.RotationDegrees = angularRot1;
		ring2.RotationDegrees = angularRot2;
	}
}
