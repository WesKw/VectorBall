using Godot;
using System;

public class Main : Spatial
{
	private int levelID = 1;
	private PackedScene currentLevel;
	private PackedScene player;
	private Camera cam;
	private LevelController controller;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		controller = GetNode("LevelController") as LevelController;
		LoadLevel(levelID);
		LoadPlayer();
	}
	
	//Load instance of level based on level ID
	private void LoadLevel(int id)
	{
		string levelPath = "res://scenes/Levels/level" + id.ToString() + ".tscn";
		currentLevel = (PackedScene)ResourceLoader.Load(levelPath);
		Level levelInstance = (Level)currentLevel.Instance();
		levelInstance.Name = "level" + id.ToString();
		AddChild(levelInstance);
		controller.CurrentLevel = levelInstance.GetNode<KinematicBody>("Geometry/KinematicBody");
	}
	
	private void LoadPlayer()
	{
		string playerPath = "res://scenes/Player/Player.tscn";
		player = (PackedScene)ResourceLoader.Load(playerPath);
		Player playerInstance = (Player)player.Instance();
		playerInstance.Name = "Player";
		//cam = playerInstance.GetNode<Camera>("CameraPivot");
		AddChild(playerInstance);
	}
}
