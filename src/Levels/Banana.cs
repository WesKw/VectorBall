using Godot;
using System;

public class Banana : GameObject
{
	Vector3 originalRotation;
	SwitchEmitter attachedSwitch;
	Spatial geometry;
	
	protected override void SwitchPressed() => doAction = true;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		geometry = GetNode("Geometry") as Spatial;
		attachedSwitch = GetNode("Switch") as SwitchEmitter;
		doAction = false;
		originalRotation = geometry.RotationDegrees;
		startPos = geometry.Translation;
	}

	public override void _PhysicsProcess(float delta)
	{
		if(doAction)
		{
			float speed = .2f;
			Vector3 geometryRot = geometry.RotationDegrees;
			Vector3 geometryTranslation = geometry.Translation;
			geometryRot.y = Mathf.Lerp(geometryRot.y, 90f, speed);
			geometryTranslation.x = Mathf.Lerp(geometryTranslation.x, 35f, speed);
			geometryTranslation.z = Mathf.Lerp(geometryTranslation.z, -28f, speed);
			geometry.RotationDegrees = geometryRot;
			geometry.Translation = geometryTranslation;
			if(geometryRot.y >= 90.0f) doAction = false;
		}
	}
	
	protected override void Reset()
	{
		geometry.Translation = startPos;
		geometry.RotationDegrees = originalRotation;
		attachedSwitch.Reset();
		doAction = false;
	}
}
