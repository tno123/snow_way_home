using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_start_button_pressed()
	{
		var sceneManager = GetNode<SceneManager>("/root/SceneManager");
		//Todo: update this to use a save slot screen
		sceneManager.GotoScene("res://scenes/levels/level1/Level_1.tscn");
	}

}
