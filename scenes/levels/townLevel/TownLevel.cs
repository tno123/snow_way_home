using Godot;
using System;

public partial class TownLevel : Node
{
	AnimatedSprite2D Exclamation;
	// Called when the node enters the scene tree for the first time.
	Camera2D camera;
	CharacterBody2D snowball;
	
	private Vector2 defaultZoom = new Vector2(1.0f, 1.0f);
	private Vector2 boostZoom = new Vector2(1.2f, 1.2f);
	private float zoomSpeed = 0.05f;  // Determines how fast the zoom changes; adjust as needed
	
	
	public override void _Ready()
	{
		camera = GetNode<Camera2D>("Camera2D");
		Exclamation = GetNode<Node>("UI").GetNode<AnimatedSprite2D>("Exclamation");
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
	
		await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
		Exclamation.Play("default");
	}
}
