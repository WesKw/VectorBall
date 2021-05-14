using Godot;
using System;

public abstract class SwitchEmitter : StaticBody
{
	[Signal]
	public delegate void switch_pressed();
	
	protected bool pressed = false;
	
	protected virtual void ConnectToParent()
	{
		GameObject parent = GetNode("..") as GameObject;
		Connect("switch_pressed", parent, "SwitchPressed");
	}
	
	public virtual void Reset()
	{
		pressed = false;
	}
}
