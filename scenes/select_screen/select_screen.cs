using Godot;
using System;

public partial class select_screen : Control
{
	private AnimationPlayer animationPlayer;
	private string levelSelected;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void _on_button_pressed(string buttonName, string level=null)
	{
	
		GD.Print(buttonName);
		GD.Print(level);
		if (animationPlayer != null){
			animationPlayer.Stop();
		}
		string path = buttonName.ToString() + "/blink";
		animationPlayer = GetNode<AnimationPlayer>(path);
		//levelSelected="res://scenes/levels/level1/Level_1.tscn";
		levelSelected=level;
		animationPlayer.Play("new_animation");

	}
	private void _on_start_pressed()
	{
	
		if (levelSelected != null){
			GD.Print(levelSelected != null);

			var sceneManager = GetNode<SceneManager>("/root/SceneManager");
			sceneManager.GotoScene(levelSelected);
		}

	}
}






