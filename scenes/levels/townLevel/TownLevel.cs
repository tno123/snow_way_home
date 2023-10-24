using Godot;
using System;

public partial class TownLevel : Node
{
	// Called when the node enters the scene tree for the first time.
	AnimatedSprite2D boostInfo;
	ProgressBar SnowPower;

	//bool playing = false;

	public override void _Ready()
	{
		boostInfo = GetNode<CanvasLayer>("UI").GetNode<AnimatedSprite2D>("boostInfo");
		SnowPower = GetNode<CanvasLayer>("UI").GetNode<ProgressBar>("SnowPower");
		boostInfo.Hide();
		SnowPower.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_down"))
		{
			GD.Print("pressed");
			boostInfo.Show();
			SnowPower.Show();
			boostInfo.Play("default");
			/*  if (!playing)
			 {
				 boostInfo.Play("default");
				 playing = true;
			 } */
		}

		//boostInfo.Hide();
		//boostInfo.Stop();
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
