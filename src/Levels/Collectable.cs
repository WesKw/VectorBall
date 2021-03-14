using Godot;
using System;

public class Collectable : Area
{
	public override void _PhysicsProcess(float delta)
	{
		Vector3 rot = RotationDegrees;
		rot.y += delta;
	}
	
	private void _on_Area_body_entered(object body)
	{
		if(body is PlayerBody)
		{
			QueueFree();
		}
	}
}
