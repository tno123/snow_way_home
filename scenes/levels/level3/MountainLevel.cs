using Godot;
using System;

public partial class MountainLevel : Node
{
	[Export]
	public Polygon2D polygon;

	[Export]
	public CollisionPolygon2D collisionPolygon;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		polygon.Polygon = collisionPolygon.Polygon;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
