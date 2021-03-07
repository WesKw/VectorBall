using Godot;
using System;

public class Level : Spatial
{
	[Signal]
	public delegate void level_finished();
	
	[Signal]
	public delegate void fall_out();
	
	//private Vector3 stageLocation;
	//private Vector3 startLocation;
	private RigidBody player;
	private ObjectManager objectM;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Area>("DeathBarrier/DeathArea").Connect("body_entered", this, nameof(_on_DeathArea_area_entered));
		player = GetParent().GetNode("Player/RigidBody") as RigidBody;
		GetNode<Area>("Geometry/Goal/Area").Connect("body_entered", this, nameof(_on_Goal_entered));
		objectM = GetNodeOrNull("ObjectManager") as ObjectManager;
	}

	public override void _PhysicsProcess(float delta)
	{
		
	}
	
	private void _on_DeathArea_area_entered(object body)
	{
		if(body is PlayerBody)
		{
			GD.Print("Fall out!");
			EmitSignal(nameof(fall_out));
			//Object manager will only exist on stages with moving parts or objects
			if(!(objectM is null))
				objectM.EmitSignal("on_reset");
		}
	}
	
	private void _on_Goal_entered(object body)
	{
		if(body is RigidBody)
		{
			GD.Print("Next level!");
			EmitSignal(nameof(level_finished));
		}
	}
}
