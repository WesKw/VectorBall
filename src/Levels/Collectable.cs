using Godot;
using System;

public class Collectable : Area
{
	[Export]
	float speed = 100.0f;
	AnimationPlayer animator;
	
	public override void _Ready()
	{
		animator = GetNode("AnimationPlayer") as AnimationPlayer;
	}
	
	public override void _PhysicsProcess(float delta)
	{
		Vector3 rot = RotationDegrees;
		rot.y += delta * speed;
		RotationDegrees = rot;
	}
	
	private void _on_Collectable_body_entered(object body)
	{
		if(body is PlayerBody) { animator.Play("Collected"); }
	}
	
	private void _on_AnimationPlayer_animation_finished(String anim_name)
	{
		if(anim_name == "Collected") GD.Print("Remove");
	}
}



