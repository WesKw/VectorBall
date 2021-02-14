using Godot;
using System;

public class Title : Control
{
	private enum Btn
	{
		Start,
		Exit
	}
	
	private Btn currentBtn;
	private VBoxContainer btnContainer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentBtn = Btn.Start;
		btnContainer = GetNode("MarginContainer/Buttons/VBoxContainer") as VBoxContainer;
		btnContainer.GetChild<Button>((int)currentBtn).Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		btnContainer.GetChild<Button>(1).Modulate = new Color(1.0f, 1.0f, 1.0f, 0.5f);
	}
	
	public override void _Process(float delta)
	{
		if(Input.IsActionJustPressed("ui_accept"))
		{
			btnContainer.GetChild<Button>((int)currentBtn).EmitSignal("pressed");
		}
	}
	
	//Handles switching menu options
	public override void _UnhandledInput(InputEvent @event)
	{
		Btn previousBtn = currentBtn;
		
		if(Input.IsActionJustPressed("ui_up"))
		{
			currentBtn--;
			if((int)currentBtn < 0)
				currentBtn = Btn.Exit;
		} else if(Input.IsActionJustPressed("ui_down"))
		{
			currentBtn++;
			if((int)currentBtn > 1)
				currentBtn = Btn.Start;
		}
		
		if(Input.IsKeyPressed(70))
			OS.WindowFullscreen = !OS.WindowFullscreen;
		
		btnContainer.GetChild<Button>((int)previousBtn).Modulate = new Color(1.0f, 1.0f, 1.0f, 0.5f);
		btnContainer.GetChild<Button>((int)currentBtn).Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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
