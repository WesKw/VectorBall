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
	//private RigidBody player;
	//private float maxTilt;
	private ObjectManager objectM;
	private WorldTilt world;
	
	/*
	public float MaxTilt
	{
		get { return maxTilt; }
		set { maxTilt = value; }
	}
	*/
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Area>("DeathBarrier/DeathArea").Connect("body_entered", this, nameof(_on_DeathArea_area_entered));
		//player = GetParent().GetNode("Player/RigidBody") as RigidBody;
		objectM = GetNodeOrNull("ObjectManager") as ObjectManager;
		//Connect("tree_exiting", this, "on_despawn");
		//world = GetNode<WorldTilt>("WorldEnv");
	}
	
	private void _on_DeathArea_area_entered(object body)
	{
		if(body is PlayerBody) EmitSignal(nameof(fall_out));
	}
	
	private void _on_Goal_entered(int levelModifier) => EmitSignal(nameof(level_finished), levelModifier);
	
	public void EmitObjectManagerSignal()
	{
		if(!(objectM is null))
		{
			GD.Print("Send reset signal");
			objectM.EmitSignal("on_reset");
		}
	}
	
	public void SetupWorld(float tilt, Spatial playerCam)
	{
		if(world != null) 
		{
			world.MaxTilt = tilt;
			world.CamPivot = playerCam;
		}
	}
}
