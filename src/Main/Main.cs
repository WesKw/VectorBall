using Godot;
using System;

public class Main : Spatial
{
	private string[] levelList;
	private int levelID = 1;
	private PackedScene currentLevel;
	private PackedScene player;
	private Spatial cam;
	private LevelController controller;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		controller = GetNode("LevelController") as LevelController;
		LoadPlayer();
		LoadLevel(levelID);
	}
	
	//Load instance of level based on level ID
	private void LoadLevel(int id)
	{
		string levelPath = "res://scenes/Levels/level" + id.ToString() + ".tscn";
		currentLevel = (PackedScene)ResourceLoader.Load(levelPath);
		Level levelInstance = (Level)currentLevel.Instance();
		levelInstance.Name = "level" + id.ToString();
		AddChild(levelInstance);
		controller.CurrentLevel = levelInstance.GetNode<Spatial>("Geometry");
		controller.PivotPoint = levelInstance.GetNode<Spatial>("PivotPoint");
	}
	
	private void LoadPlayer()
	{
		string playerPath = "res://scenes/Player/Player.tscn";
		player = (PackedScene)ResourceLoader.Load(playerPath);
		Player playerInstance = (Player)player.Instance();
		playerInstance.Name = "Player";
		//cam = playerInstance.GetNode<Camera>("CameraPivot");
		AddChild(playerInstance);
		cam = playerInstance.GetNode("CameraPivot") as Spatial;
		controller.PlayerCamPivot = cam;
	}
}
