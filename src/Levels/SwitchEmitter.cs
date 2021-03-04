using Godot;
using System;

public abstract class SwitchEmitter : StaticBody
{
	[Signal]
	public delegate void switch_pressed();
	
	protected bool pressed = false;
	
	protected abstract void SwitchAction();
	protected abstract void FindSwitchObjects();
	
	public virtual void Reset()
	{
		pressed = false;
		
	}
}
