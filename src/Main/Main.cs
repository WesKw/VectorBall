using Godot;
using System;

public class Main : Spatial
{
	//An array that contains every level name
	private string[] levelList = {  "Beginner", "Split", "Downhill", "Uphill", "Sideways",
									"Elevator", "Button", "Rotation", "Airtime", "Exam", 
									"Pitstop", "Hole-in-one" };
									
	private bool reset;
	[Export]
	private int levelID = 1;
	private int currentTime = 60;
	
	//private LevelController controller;
	private PackedScene currentLevel;
	private PackedScene playerScene;
	private Spatial cam;
	private Player player;
	private Level level;
	private GameUI gameUI;
	
	System.Timers.Timer loadTimer = new System.Timers.Timer(5000);
	System.Timers.Timer fallTimer = new System.Timers.Timer(3500);
	System.Timers.Timer levelTimer;
	//System.Timers.Timer startTimer = new System.Timers.Timer(5000);
	
	public bool Reset
	{
		get { return reset; }
		set { reset = value; }
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//controller = GetNode("LevelController") as LevelController;
		gameUI = GetNode<GameUI>("GameUI");
		loadTimer.AutoReset = false;
		loadTimer.Enabled = false;
		loadTimer.Elapsed += LoadTimerElapsed;
		//controller = GetNode("LevelController") as LevelController;
		fallTimer.AutoReset = false;
		fallTimer.Elapsed += FallTimerElapsed;
		//startTimer.AutoReset = false;
		//startTimer.Elapsed += StartTimerElapsed;
		LoadPlayer();
		LoadLevel(levelID);
	}
	
	public override void _Process(float delta)
	{
		gameUI.UpdatePlayerTranslation(player.PlayerBodyTranslation);
	}
	
	private void CheckConnections()
	{
		if(!level.IsConnected("fall_out", this, nameof(on_fall_out))) 
			level.Connect("fall_out", this, nameof(on_fall_out));
			
		if(!level.IsConnected("level_finished", this, nameof(on_level_finished)))
			level.Connect("level_finished", this, nameof(on_level_finished));
			
		if(!level.IsConnected("ready", this, nameof(on_level_ready)))
			level.Connect("ready", this, nameof(on_level_ready));
	}
	
	//Load instance of level based on level ID
	private void LoadLevel(int id)
	{
		string levelPath = "res://scenes/Levels/Stages/level" + id.ToString() + ".tscn";
		currentLevel = (PackedScene)ResourceLoader.Load(levelPath);
		level = (Level)currentLevel.Instance();
		level.Name = "level" + id.ToString();
		AddChild(level);
		CheckConnections();
		player.PlayIntro("CameraSwirl");
		gameUI.UpdateLevelName(levelList[id-1]);
		//startTimer.Enabled = true;
		//controller.CurrentLevel = level.GetNode("Geometry") as Spatial;
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
		//controller.PivotPoint = player.GetNode<PlayerBody>("RigidBody");
	}
	
	private void on_level_finished(int levelModifier)
	{
		levelTimer.Stop();
		player.Goal();
		//GD.Print("level finished!");
		UpdateUIScore(currentTime * 1000);
		levelID += levelModifier;
		loadTimer.Enabled = true;
	}
	
	private void LoadTimerElapsed(System.Object source, System.Timers.ElapsedEventArgs e)
	{
		gameUI.PlayAnimation("FadeOut");
		if(levelID > levelList.Length)
		{
			GetTree().ChangeScene("res://scenes/Title/Title.tscn");
		}
		else
		{
			GD.Print("Load next level now");
			//Remove current level
			level.QueueFree();
			player.Reset();
			LoadLevel(levelID);
			gameUI.PlayAnimation("FadeIn");
		}
	}
	
	private void on_fall_out()
	{
		levelTimer.Stop();
		Reset = true;
		fallTimer.Enabled = true;
		player.FallOut();
	}
	
	private void on_reset()
	{
		gameUI.PlayAnimation("FadeOut");;
		Reset = false;
		currentTime = 60;
		gameUI.UpdateTimer(currentTime);
		gameUI.PlayAnimation("FadeIn");
		player.PlayIntro("CameraSwirl");
	}
	
	private void on_level_ready()
	{
		gameUI.Reset();
		gameUI.PlayAnimation("FadeIn");
	}
	
	private void FallTimerElapsed(System.Object source, System.Timers.ElapsedEventArgs e)
	{
		player.FallTimerElapsed();
		level.EmitObjectManagerSignal();
	}
	
	private void PlayerAnimFinished(string animName)
	{
		if(animName == "CameraSwirl")
		{
			gameUI.PlayAnimation("GO!");
			player.Start();
			levelTimer = new System.Timers.Timer(60000);
			levelTimer.Elapsed += UpdateUITimer;
			levelTimer.Interval = 1000;
			levelTimer.AutoReset = false;
			levelTimer.Start();
		}
	}
	
	private void UpdateUIScore(int val)
	{
		gameUI.UpdateScore(val);
	}
	
	private void UpdateUITimer(System.Object source, System.Timers.ElapsedEventArgs e)
	{
		currentTime -= 1;
		if(currentTime < 0)
		{
			//GameUI.PlayAnimation("TimeOver");
			player.BodyLock(true);
			on_fall_out();
		}
		else
		{
			levelTimer.Start();
			gameUI.UpdateTimer(currentTime);
		}
	}
}
