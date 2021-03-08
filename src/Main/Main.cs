using Godot;
using System;

public class Main : Spatial
{
	//An array that contains every level name
	private string[] levelList = {  "Beginner", "Split", "Downhill", "Uphill", "Sideways",
									"Elevator", "Button", "Rotation", "Airtime", "Exam" };
									
	private bool reset;
	[Export]
	private int levelID = 1;
	
	private PackedScene currentLevel;
	private PackedScene playerScene;
	private Spatial cam;
	private Player player;
	private Level level;
	private GameUI gameUI;
	private AnimationPlayer UIAnimator;
	
	System.Timers.Timer levelTimer = new System.Timers.Timer(60000);
	System.Timers.Timer loadTimer = new System.Timers.Timer(5000);
	System.Timers.Timer fallTimer = new System.Timers.Timer(3500);
	
	public bool Reset
	{
		get { return reset; }
		set { reset = value; }
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameUI = GetNode<GameUI>("GameUI");
		UIAnimator = gameUI.GetNode("AnimationPlayer") as AnimationPlayer;
		loadTimer.AutoReset = false;
		loadTimer.Enabled = false;
		loadTimer.Elapsed += LoadTimerElapsed;
		//controller = GetNode("LevelController") as LevelController;
		fallTimer.AutoReset = false;
		fallTimer.Elapsed += FallTimerElapsed;
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
		UIAnimator.Play("FadeOut");
		if(levelID > levelList.Length)
		{
			GetTree().ChangeScene("res://scenes/Title/Title.tscn");
		}
		else
		{
			GD.Print("Load next level now");
			//Remove current level
			level.QueueFree();
			LoadLevel(levelID);
			player.Reset();
			UIAnimator.Play("FadeIn");
		}
	}
	
	private void on_fall_out()
	{
		Reset = true;
		fallTimer.Enabled = true;
		player.FallOut();
	}
	
	private void on_reset()
	{
		UIAnimator.Play("FadeOut");;
		Reset = false;
		UIAnimator.Play("FadeIn");
	}
	
	private void on_level_ready()
	{
		UIAnimator.Play("FadeIn");
	}
	
	private void FallTimerElapsed(System.Object source, System.Timers.ElapsedEventArgs e)
	{
		player.FallTimerElapsed();
		level.EmitObjectManagerSignal();
	}
}
