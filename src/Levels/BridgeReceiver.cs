using Godot;
using System;

public class BridgeReceiver : AbstractSwitchReceiver
{
	Vector3 rot;
	
	public override void _Ready()
	{
		doAction = false;
		rot = RotationDegrees;
	}
	
	public override void _PhysicsProcess(float delta)
	{
		if(doAction)
		{
			rot.y = Mathf.Lerp(rot.y, 0.0f, .2f);
			RotationDegrees = rot;
			if(rot.y >= 90.0f) doAction = false;
		}
	}
	
	protected override void SwitchAction()
	{
		
	}
	
	public override void on_switch_pressed()
	{
		doAction = true;
	}
	
	public override void Reset()
	{
		
	}
}
