using Godot;
using System;

public partial class MainMenu : Control
{
	Texture2D StartSelected;
	Texture2D StartUnselected;
	Texture2D OptionsSelected;
	Texture2D OptionsUnselected;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var StartButtonTexture = GetNode<TextureRect>("Start/StartTexture");
		StartSelected =  (Texture2D)ResourceLoader.Load("res://art/ui/startbutton_selected.png");
		StartUnselected = (Texture2D)ResourceLoader.Load("res://art/ui/startbutton.png");
		OptionsSelected = (Texture2D)ResourceLoader.Load("res://art/ui/optionsbutton_selected.png");
		OptionsUnselected = (Texture2D)ResourceLoader.Load("res://art/ui/optionsbutton.png");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void _on_start_button_pressed()
	{
		var sceneManager = GetNode<SceneManager>("/root/SceneManager");
		//Todo: update this to use a save slot screen
		sceneManager.GotoScene("res://scenes/levels/level1/Level_1.tscn");
	}
	
	public override void _Process(double delta)
	{
	}
	private void _on_start_button_down()
	{
		var StartButtonTexture = GetNode<TextureRect>("Start/StartTexture");
		StartButtonTexture.Texture = StartSelected;	
	
	}
	
	
	private void _on_start_button_up()
	{
		var StartButtonTexture = GetNode<TextureRect>("Start/StartTexture");
		StartButtonTexture.Texture = StartUnselected;	
	}
	
	private void _on_options_button_down()
	{
		var OptionsButtonTexture = GetNode<TextureRect>("Options/OptionsTexture");
		OptionsButtonTexture.Texture = OptionsSelected;
	}
	
	private void _on_options_button_up()
	{
		var OptionsButtonTexture = GetNode<TextureRect>("Options/OptionsTexture");
		OptionsButtonTexture.Texture = OptionsUnselected;
	}
}














