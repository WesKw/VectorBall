using Godot;
using System;

public class PlayerBody : RigidBody
{
	private bool reset;
	private bool intro;
	
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
			t.origin.y = 5.0f;
			t.origin.x = 0;
			t.origin.z = 0;
			state.Transform = t;
			Reset = false; 
		}
	}
	
}
