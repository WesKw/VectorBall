using Godot;
using System;

public class GameUI : Control
{
	Vector3 updatedTranslation;
	Vector3 camTranslation;
	Vector3 camRotation;
	Camera overHeadCam;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		overHeadCam = GetNode("MarginContainer/CameraView/ViewportContainer/Viewport/OHCamera") as Camera;
		camTranslation = overHeadCam.Translation;
		camRotation = overHeadCam.RotationDegrees;
	}
	
	public override void _Process(float delta)
	{
		overHeadCam.Translation = camTranslation;
	}
	
	public void UpdatePlayerTranslation(Vector3 playerLoc)
	{
		camTranslation.x = playerLoc.x;
		camTranslation.y = playerLoc.y + 12;
		camTranslation.z = playerLoc.z;
	}
}
