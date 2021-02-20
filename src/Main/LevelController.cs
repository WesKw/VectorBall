using Godot;
using System;

public class LevelController : Node
{
	private bool inputAllowed = true;
	private float speed = .5f;
	private float maxTilt = Mathf.Deg2Rad(8.0f);
	private Spatial currentLevel = null;
	private Spatial pivotPoint;
	private Spatial playerCamPivot = null;
	private Vector3 tilt;
	
	public Spatial CurrentLevel
	{
		get {return currentLevel;}
		set {currentLevel = value;}
	}
	
	public bool InputAllowed
	{
		get { return inputAllowed; }
		set { inputAllowed = value; }
	}
	
	public Spatial PivotPoint
	{
		get { return pivotPoint; }
		set { pivotPoint = value; }
	}
	
	public Spatial PlayerCamPivot
	{
		get { return playerCamPivot; }
		set { playerCamPivot = value; }
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tilt = new Vector3();
	}

	public override void _PhysicsProcess(float delta)
	{
		if(inputAllowed)
		{
			float horiz = Input.GetActionStrength("tilt_right") - Input.GetActionStrength("tilt_left");
			float vert = Input.GetActionStrength("tilt_up") - Input.GetActionStrength("tilt_down");
			
			TiltLevel(currentLevel, pivotPoint, Vector3.Right, vert * maxTilt);
			TiltLevel(currentLevel, pivotPoint, Vector3.Back, horiz * maxTilt);
		}
	}
	
	//Algorithm for rotating around pivot
	//Thanks to path2963 for this
	//Translated to C# from gdscript
	private void TiltLevel(Spatial geometry, Spatial point, Vector3 axis, float angle)
	{
		float rot;
		
		//Use x or z based on axis passed in
		if(axis == Vector3.Right)
			rot = angle + geometry.Rotation.x;
		else
			rot = angle + geometry.Rotation.z;
		
		//Initial start point
		Vector3 start = point.Translation;
		geometry.GlobalTranslate(-start);	//translate level using geometry
		geometry.Transform = geometry.Transform.Rotated(axis, -rot);	//Rotate based on axis and rotation
		geometry.GlobalTranslate(start);	//Return level to original location
	}
	
	//Reload the current scene based on button press
	public override void _UnhandledInput(InputEvent @event)
	{
		if(Input.IsActionJustPressed("reset"))
		{
			GetTree().ReloadCurrentScene();
		}
	}
}
