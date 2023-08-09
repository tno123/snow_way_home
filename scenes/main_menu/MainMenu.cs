using Godot;
using System;

public partial class MainMenu : Control
{
	Texture2D StartSelected;
	Texture2D StartUnselected;
	Texture2D OptionsSelected;
	Texture2D OptionsUnselected;
	
	TextureRect StartButtonTexture;
	TextureRect OptionsButtonTexture;
	
	[Export]
	public Transitions StartTransition;
	[Export]
	public Transitions LevelsTransition;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StartTransition.Visible = LevelsTransition.Visible = true;
		StartButtonTexture = GetNode<TextureRect>("Start/StartTexture");
		OptionsButtonTexture = GetNode<TextureRect>("Options/OptionsTexture");

		StartSelected =  (Texture2D)ResourceLoader.Load("res://art/ui/startbutton_selected.png");
		StartUnselected = (Texture2D)ResourceLoader.Load("res://art/ui/startbutton.png");
		OptionsSelected = (Texture2D)ResourceLoader.Load("res://art/ui/optionsbutton_selected.png");
		OptionsUnselected = (Texture2D)ResourceLoader.Load("res://art/ui/optionsbutton.png");
	}

	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	
	public override void _Process(double delta)
	{
	}
	private void _on_start_button_pressed()
	{
		
		StartTransition.SetNextAnimation("fade_out");
		GetNode<TextureRect>("Start/StartTexture").Visible=false;
		GetNode<TextureRect>("Options/OptionsTexture").Visible=false;
		GetNode<TextureRect>("Levels/LevelsTexture").Visible=false;
		//var sceneManager = GetNode<SceneManager>("/root/SceneManager");
		//Todo: update this to use a save slot screen
		//sceneManager.GotoScene("res://scenes/levels/level1/Level_1.tscn");
	}
	private void _on_start_button_down()
	{
		
		StartButtonTexture.Texture = StartSelected;	
	}
	
	private void _on_start_button_up()
	{
	
		StartButtonTexture.Texture = StartUnselected;	
	}
	
	private void _on_options_button_down()
	{
		
		OptionsButtonTexture.Texture = OptionsSelected;
	}
	
	private void _on_options_button_up()
	{
		OptionsButtonTexture.Texture = OptionsUnselected;
	}
	
	private void _on_levels_button_pressed()
	{
		LevelsTransition.SetNextAnimation("fade_out");

	}
}






