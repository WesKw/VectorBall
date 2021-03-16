using Godot;
using System;

public class PlayerBody : RigidBody
{
	private bool reset;
	private bool intro;
	private Vector3 startPos = new Vector3(0, 2.0f, 0);
	
	public bool Reset
	{
		get { return reset; }
		set { reset = value; }
	}
	
	public bool Intro
	{
		get { return intro; }
		set { intro = value; }
	}
	
	public override void _IntegrateForces(PhysicsDirectBodyState state) 
	{ 
		if(Reset) 
		{
			Transform t = state.Transform;
			state.LinearVelocity *= 0;
			state.AngularVelocity *= 0;
			t.origin.y = startPos.y;
			t.origin.x = startPos.x;
			t.origin.z = startPos.z;
			state.Transform = t;
			Reset = false; 
		}
	}
	
}
