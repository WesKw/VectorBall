using Godot;
using System;

public abstract class GameObject : Spatial
{
	protected bool doAction = false;
	
	protected abstract void SwitchPressed();
	//public abstract void on_switch_pressed();
	
	protected Vector3 currPos;
	protected Vector3 startPos;
	protected Vector3 rot = Vector3.Zero;
	
	protected abstract void Reset();
}
