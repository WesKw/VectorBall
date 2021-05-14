using Godot;
using System;
using System.Linq;

public class PlaySwitch : SwitchEmitter
{
	public override void _Ready()
	{
		ConnectToParent();
		pressed = false;
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



