using Godot;
using System;
using System.Linq;

public class PlaySwitch : SwitchEmitter
{
	protected override void SwitchAction()
	{
		
	}
	
	protected override void FindSwitchObjects()
	{
		foreach(Node x in GetNode<Spatial>("..").GetChildren())
		{
			if(x is AbstractSwitchReceiver)
			{
				Connect("switch_pressed", x, "on_switch_pressed");
			}
		}
	}
	
	public override void _Ready()
	{
		FindSwitchObjects();
	}
	
	private void _on_Area_body_entered(object body)
	{
		if(pressed == false && body is PlayerBody)
		{
			pressed = true;
			EmitSignal("switch_pressed");
		}
	}
}



