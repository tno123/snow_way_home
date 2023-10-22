using Godot;
using System;

public partial class TownLevel : Node
{
	
	// Called when the node enters the scene tree for the first time.
	AnimatedSprite2D boostInfo;
	public override void _Ready()
	{
		boostInfo = GetNode<AnimatedSprite2D>("boostInfo");
	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		boostInfo.Play("default");
		/*
		if ((snowball as Snowball).IsBoosting)
			{
				camera.Zoom = camera.Zoom.Lerp(boostZoom, zoomSpeed);
				//camera.ZoomIn();
			}
			else
			{
				camera.Zoom = camera.Zoom.Lerp(defaultZoom, zoomSpeed);
				//camera.ZoomOut();
			}
			*/
	}
	
	
}
