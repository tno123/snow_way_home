using Godot;
using System;

public partial class CameraScript : Camera2D
{
	private Vector2 defaultZoom = new Vector2(1.0f, 1.0f);
	private Vector2 boostZoom = new Vector2(1.2f, 1.2f);
	private float zoomSpeed = 0.05f;  // Determines how fast the zoom changes; adjust as needed
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void ZoomIn(){
		//Zoom.Lerp(boostZoom, zoomSpeed);
	}
	public void ZoomOut(){
		//Zoom.Lerp(defaultZoom, zoomSpeed);
	}
}
