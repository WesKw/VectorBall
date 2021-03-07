using Godot;
using System;

public abstract class AbstractSwitchReceiver : GameObject
{
	protected bool doAction = false;
	
	protected abstract void SwitchAction();
	public abstract void on_switch_pressed();
}
