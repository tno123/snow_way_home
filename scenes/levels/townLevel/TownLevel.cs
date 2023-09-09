using Godot;
using System;

public partial class TownLevel : Node
{
	AnimatedSprite2D Exclamation;
	// Called when the node enters the scene tree for the first time.
	
	public override void _Ready()
	{
	
		Exclamation = GetNode<AnimatedSprite2D>("Exclamation");
		SomeFunction();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
	
	public async void SomeFunction() //https://ask.godotengine.org/7042/wait-like-function
	{
	
		
	}
}
