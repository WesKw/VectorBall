using Godot;
using System;
using System.Linq;

public class ObjectManager : Node
{
	[Signal]
	public delegate void on_reset();
	
	private void FindResettableObjects(Spatial geometry)
	{
		foreach(Node x in geometry.GetChildren())
		{
			if((x is GameObject))
				Connect("on_reset", x, "Reset");
		}
	}
	
	public override void _Ready()
	{
		Spatial geometry = GetNode("../Geometry") as Spatial;
		FindResettableObjects(geometry);
	}
}
