using Godot;
using System;

public class BridgeReceiver : GameObject
{
	KinematicBody bridge;
	Vector3 startingRot;
	SwitchEmitter attachedSwitch;
	
	public override void _Ready()
	{
		attachedSwitch = GetNode("Switch") as SwitchEmitter;
		bridge = GetChild(0) as KinematicBody;
		doAction = false;
		startingRot = bridge.RotationDegrees;
		rot = startingRot;
	}
	
	public override void _PhysicsProcess(float delta)
	{
		if(doAction)
		{
			rot.y = Mathf.Lerp(rot.y, 0.0f, .2f);
			bridge.RotationDegrees = rot;
			if(rot.y >= 90.0f) doAction = false;
		}
	}
	
	protected override void SwitchPressed() { doAction = true; GD.Print("Good"); }
	
	//public override void on_switch_pressed() => doAction = true;
	
	protected override void Reset()
	{
		attachedSwitch.Reset();
		//GD.Print("Reset bridge?");
		doAction = false;
		rot = startingRot;
		bridge.RotationDegrees = startingRot;
	}
}
