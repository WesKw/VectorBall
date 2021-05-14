using Godot;
using System;

public abstract class GameObject : Spatial
{
	protected bool doAction = false;		//game object performs action if true, used for switches
	protected Vector3 currPos;				//current position
	protected Vector3 startPos;				//object's starting position
	protected Vector3 rot = Vector3.Zero;	//original rotation
	
	protected abstract void Reset();		//called when level resets
	protected abstract void SwitchPressed();//called when an attached switch is pressed
}
