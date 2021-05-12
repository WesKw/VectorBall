using Godot;
using System;

public class Title : Control
{
	private VBoxContainer btnContainer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		btnContainer = GetNode("MarginContainer/Buttons/VBoxContainer") as VBoxContainer;
		GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeIn");
		//GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play(0);
		btnContainer.GetChild<Button>(0).GrabFocus();
	}
	
	//Handles switching menu options
	public override void _UnhandledInput(InputEvent @event)
	{
		//if(Input.IsActionJustPressed("toggle_fullscreen"));
			//OS.WindowFullscreen = !OS.WindowFullscreen;
	}
	
	#region ButtonPresses
	private void _on_Start_pressed() => GetTree().ChangeScene("res://scenes/Main.tscn");
	private void _on_LevelSelect_pressed() => GD.Print("Level Select");
	private void _on_Exit_pressed() => GetTree().Quit();
	#endregion
}
