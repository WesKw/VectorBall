using Godot;
using System;

public class PlayerBody : RigidBody
{
	private bool reset;
	private bool intro;
	private Vector3 startPos = new Vector3(0, 3.0f, 0);
	[Export]
	private float maxSpeed = 15f;
	
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
		Vector3 velocity = state.LinearVelocity;
		
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
		
		//Velocity checks, TODO: Apply damping instead of setting velocity
		if(velocity.Length() > maxSpeed && Mathf.Abs(velocity.y) <= 2f) 
		{
			velocity = velocity.Normalized() * maxSpeed;
		} else if(velocity.Length() > maxSpeed * 2f && Mathf.Abs(velocity.y) <= 20f)
		{
			velocity = velocity.Normalized() * maxSpeed * 2f;
		} else if(velocity.Length() > maxSpeed * 3f && Mathf.Abs(velocity.y) <= 40f)
		{
			velocity = velocity.Normalized() * maxSpeed * 3f;
		}
			
		state.LinearVelocity = velocity;
		//GD.Print(state.LinearVelocity.Length());
	}
	
}


