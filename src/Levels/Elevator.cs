using Godot;
using System;

public class Elevator : GameObject
{
	float time = 0;
	[Export]
	float freq = 1;
	[Export]
	float amp = 14;
	Vector3 velocity = new Vector3(0, 0, 0);
	KinematicBody elevator;
	
	protected override void SwitchPressed() {}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		elevator = GetNode("ElevatorBody") as KinematicBody;
		startPos = Translation;
		currPos = startPos;
		GD.Print(elevator);
	}
	
	private float SetYVel(float time)
	{
		return (float)Math.Sin(time * freq) * amp;
	}

	public override void _PhysicsProcess(float delta)
	{
		Vector3 elevPos = Translation;
		time += delta;
		elevPos.y = Mathf.Lerp(elevPos.y, SetYVel(time), .2f);
		Translation = elevPos;
	}
	
	protected override void Reset()
	{
		//GD.Print("Elevator Reset");
		time = 0;
		//velocity.y = 0;
		currPos = startPos;
		Translation = currPos;
	}
}
