using Godot;
using System;

public class Level : Spatial
{
	[Signal]
	public delegate void level_finished();
	[Signal]
	public delegate void fall_out();
	
	private Vector3 stageLocation;
	private Vector3 startLocation;
	private Spatial pivotPoint;
	private RigidBody player;
	private KinematicBody stage;
	private CollisionShape DeathBarrier;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Area>("DeathBarrier/DeathArea").Connect("area_entered", this, nameof(_on_DeathArea_area_entered));
		//LevelController controller = GetNode<LevelController>("../LevelController");
		//controller.Connect("fall_out", controller, nameof(on_fallout));
		pivotPoint = GetNode("PivotPoint") as Spatial;
		//stage = rotatePivot.GetChild(0) as KinematicBody;
		//stage.SetAsToplevel(true);
		player = GetParent().GetNode("Player/RigidBody") as RigidBody;
	}

	public override void _PhysicsProcess(float delta)
	{
		//Vector3 position = player.Translation;
		//if(player.AngularVelocity.y < 1 && player.AngularVelocity.y > -1)
		//position.y -= .5f;
		//pivotPoint.Translation = position;
	}
	
	private void _on_DeathArea_area_entered(object area)
	{
		GD.Print("Death!");
		EmitSignal(nameof(fall_out));
	}
}
