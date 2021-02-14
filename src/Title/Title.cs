using Godot;
using System;

public class Title : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	private void _on_Start_pressed()
	{
		GetTree().ChangeScene("res://scenes/Main.tscn");
	}
	
	private void _on_Exit_pressed()
	{
		GetTree().Quit();
	}
}
