using Godot;
using System;

public class GameUI : Control
{
	bool paused = false;
	bool allowZoom = false;
	int zoomLevel = 2;
	float[] zooms = {36f, 24f, 12f};
	Label timer;
	Spatial pivot;
	Vector3 updatedTranslation;
	Vector3 camTranslation;
	Vector3 camRotation;
	Vector3 pivotRotation;
	Camera overHeadCam;
	AnimationPlayer animator;
	
	[Signal]
	public delegate void ChangeLevel(string levelName);
	
	private void UpdateCamTransform()
	{
		if(allowZoom)
		{
			//GD.Print("Gamer");
			camTranslation.y = Mathf.Lerp(camTranslation.y, zooms[zoomLevel], .2f);
			if(camTranslation.y == zooms[zoomLevel]) allowZoom = false;
		}
		
		float horiz = Input.GetActionStrength("tilt_right") - Input.GetActionStrength("tilt_left");
		float vert = Input.GetActionStrength("tilt_down") - Input.GetActionStrength("tilt_up");
		
		camRotation.z = 0 + (15 * horiz);
		camRotation.x = 0 - (15 * vert);
		//pivot.RotationDegrees = camRotation;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("ChangeLevel", (Owner as Main), "OnChangeLevel");
		pivot = GetNode("MarginContainer/CameraView/ViewportContainer/Viewport/CamPivot") as Spatial;
		overHeadCam = GetNode("MarginContainer/CameraView/ViewportContainer/Viewport/CamPivot/OHCamera") as Camera;
		animator = GetNode("AnimationPlayer") as AnimationPlayer;
		timer = GetNode("MarginContainer/TimerControl/Timer") as Label;
		camTranslation = overHeadCam.Translation;
		camRotation = pivot.RotationDegrees;
	}
	
	public void Reset()
	{
		Vector3 defaultRot = new Vector3(-90f, 0f, 0f);
		overHeadCam.RotationDegrees = defaultRot;
	}
	
	public override void _Process(float delta)
	{
		pivot.Translation = camTranslation;
		UpdateCamTransform();
	}
	
	public void UpdatePlayerTranslation(Vector3 playerLoc)
	{
		camTranslation.x = playerLoc.x;
		camTranslation.y = playerLoc.y + 12;
		camTranslation.z = playerLoc.z;
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if(Input.IsActionJustPressed("zoom"))
		{
			allowZoom = true;
			zoomLevel++;
			if(zoomLevel > 2)
				zoomLevel = 0;
			else if(zoomLevel < 0)
				zoomLevel = 2;
		}
		
		if(Input.IsActionJustPressed("select_level") && !paused) 
		{ 
			GetNode<PopupDialog>("MarginContainer/LevelSelect/DevMenu").Show();
			paused = true;
			GetTree().Paused = paused;
		} else if(Input.IsActionJustPressed("ui_cancel") && paused)
		{
			GetNode<PopupDialog>("MarginContainer/LevelSelect/DevMenu").Hide();
			paused = false;
			GetTree().Paused = paused;
		}
	}
	
	public void PlayAnimation(string anim, float speed = 1)
	{
		//GD.Print("Play " + anim);
		animator.Play(anim, -1f, speed);
	}
	
	private void on_update_player(float camPivot)
	{
		Vector3 camRot = overHeadCam.RotationDegrees;
		camRot.y = camPivot;
		overHeadCam.RotationDegrees = camRot;
	}
	
	public void UpdateScore(int amount)
	{
		
	}
	
	public void UpdateTimer(int amount)
	{
		timer.Text = amount.ToString();
	}
	
	public void UpdateLevelName(string levelName)
	{
		//GetNode<Label>("MarginContainer/WorldText/LevelName").Visible = false;
		GetNode<Label>("MarginContainer/WorldText/LevelName").Text = levelName;
		//animator.AssignedAnimation = "ShowLevel";
		//animator.Seek(0);
		//animator.Play("ShowLevel");
	}
	
	private void _on_Button_pressed()
	{
		paused = false;
		GetTree().Paused = paused;
		GetNode<PopupDialog>("MarginContainer/LevelSelect/DevMenu").Hide();
		string levelPath = GetNode<LineEdit>("MarginContainer/LevelSelect/DevMenu/LineEdit").Text;
		EmitSignal("ChangeLevel", levelPath);
	}
	
	private void _on_AnimationPlayer_animation_finished(String anim_name)
	{
		if(anim_name == "FadeIn")
		{
			PlayAnimation("ShowLevel");
		}
	}
}
