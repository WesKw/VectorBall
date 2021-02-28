using Godot;
using System;

public class Main : Spatial
{
	private string[] levelList = {"Beginner", "Split", "Downhill" };
	[Export]
	private int levelID = 1;
	private PackedScene currentLevel;
	private PackedScene playerScene;
	private Spatial cam;
	private Player player;
	private Level level;
	private System.Timers.Timer loadTimer = new System.Timers.Timer(5000);
	private Control gameUI;
	//private LevelController controller;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameUI = GetNode<Control>("GameUI");
		loadTimer.AutoReset = false;
		loadTimer.Enabled = false;
		loadTimer.Elapsed += LoadTimerElapsed;
		//controller = GetNode("LevelController") as LevelController;
		LoadPlayer();
		LoadLevel(levelID);
	}
	
	private void CheckConnections()
	{
		if(!level.IsConnected("fall_out", this, nameof(on_fall_out))) 
			level.Connect("fall_out", this, nameof(on_fall_out));
			
		if(!level.IsConnected("level_finished", this, nameof(on_level_finished)))
			level.Connect("level_finished", this, nameof(on_level_finished));
	}
	
	//Load instance of level based on level ID
	private void LoadLevel(int id)
	{
		string levelPath = "res://scenes/Levels/level" + id.ToString() + ".tscn";
		currentLevel = (PackedScene)ResourceLoader.Load(levelPath);
		level = (Level)currentLevel.Instance();
		level.Name = "level" + id.ToString();
		AddChild(level);
		CheckConnections();
	}
	
	private void LoadPlayer()
	{
		string playerPath = "res://scenes/Player/Player.tscn";
		playerScene = (PackedScene)ResourceLoader.Load(playerPath);
		player = (Player)playerScene.Instance();
		player.Name = "Player";
		//cam = playerInstance.GetNode<Camera>("CameraPivot");
		AddChild(player);
		cam = player.GetNode("CameraPivot") as Spatial;
		//controller.PlayerCamPivot = cam;
	}
	
	private void on_level_finished()
	{
		player.Goal();
		GD.Print("level finished!");
		levelID++;
		loadTimer.Enabled = true;
	}
	
	private void LoadTimerElapsed(System.Object source, System.Timers.ElapsedEventArgs e)
	{
		if(levelID > 3) GetTree().ChangeScene("res://scenes/Title/Title.tscn");
		else
		{
			GD.Print("Load next level now");
			//Remove current level
			level.QueueFree();
			LoadLevel(levelID);
			player.Reset();
		}
	}
	
	private void on_fall_out()
	{
		player.FallOut();
	}
}
