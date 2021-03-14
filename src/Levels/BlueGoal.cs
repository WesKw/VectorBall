using Godot;
using System;

public class BlueGoal : AbstractGoal
{
	protected override void Setup()
	{
		levelModifier = 1;
		Connect("goal_entered", Owner, "_on_Goal_entered");
	}
}
