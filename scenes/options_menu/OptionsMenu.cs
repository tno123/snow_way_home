using Godot;
using System;

public partial class OptionsMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_master_slider_value_changed(double value)
	{
		AudioServer.SetBusVolumeDb(0,(float)value);
	}


	private void _on_music_slider_value_changed(double value)
	{
		AudioServer.SetBusVolumeDb(1,(float)value);
	}


	private void _on_fx_slider_value_changed(double value)
	{
		AudioServer.SetBusVolumeDb(2,(float)value);
	}

	private void _on_back_button_pressed()
	{
		var sceneManager = GetNode<SceneManager>("/root/SceneManager");
		sceneManager.GotoScene("res://scenes/main_menu/MainMenu.tscn");
	}
}
