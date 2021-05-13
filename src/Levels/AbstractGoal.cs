using Godot;
using System;

public abstract class AbstractGoal : Spatial
{
	protected bool finished = false;
	protected int levelModifier;
	[Signal]
	public delegate void goal_entered(int modifier);
	
	private void on_goal_reached(object body)
	{
		if(body is PlayerBody && !finished)
		{
			finished = true;
			EmitSignal(nameof(goal_entered), levelModifier);
		}
	}

	protected abstract void Setup();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Area>("Area").Connect("body_entered", this, "on_goal_reached");
		Setup();
	}
}
