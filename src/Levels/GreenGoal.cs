using Godot;
using System;

public class GreenGoal : AbstractGoal
{
	protected override void Setup()
	{
		levelModifier = 2;
		Connect("goal_entered", Owner, "_on_Goal_entered");
	}
}
