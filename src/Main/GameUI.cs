using Godot;
using System;

public class GameUI : Control
{
	bool allowZoom = false;
	int zoomLevel = 2;
	float[] zooms = {36f, 24f, 12f};
	Spatial pivot;
	Vector3 updatedTranslation;
	Vector3 camTranslation;
	Vector3 camRotation;
	Vector3 pivotRotation;
	Camera overHeadCam;
	AnimationPlayer animator;
	
	private void UpdateCamTransform()
	{
		if(allowZoom)
		{
			GD.Print("Gamer");
			camTranslation.y = Mathf.Lerp(camTranslation.y, zooms[zoomLevel], .2f);
			if(camTranslation.y == zooms[zoomLevel]) allowZoom = false;
		}
		
		float horiz = Input.GetActionStrength("tilt_right") - Input.GetActionStrength("tilt_left");
		float vert = Input.GetActionStrength("tilt_down") - Input.GetActionStrength("tilt_up");
		
		camRotation.z = 0 + (15 * horiz);
		camRotation.x = 0 - (15 * vert);
		pivot.RotationDegrees = camRotation;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pivot = GetNode("MarginContainer/CameraView/ViewportContainer/Viewport/CamPivot") as Spatial;
		overHeadCam = GetNode("MarginContainer/CameraView/ViewportContainer/Viewport/CamPivot/OHCamera") as Camera;
		animator = GetNode("AnimationPlayer") as AnimationPlayer;
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
	}
	
	public void PlayAnimation(string anim)
	{
		//GD.Print("Play " + anim);
		animator.Play(anim);
	}
	
	private void on_update_player(float camPivot)
	{
		Vector3 camRot = overHeadCam.RotationDegrees;
		camRot.y = camPivot;
		overHeadCam.RotationDegrees = camRot;
	}
}
