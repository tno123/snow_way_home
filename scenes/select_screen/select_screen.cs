using Godot;
using System;

public partial class select_screen : Control
{
	private AnimationPlayer animationPlayer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void _on_button_pressed(StringName buttonName)
	{
		GD.Print(buttonName);
		if (animationPlayer != null){
			animationPlayer.Stop();
		}
		string path = buttonName.ToString() + "/blink";
		animationPlayer = GetNode<AnimationPlayer>(path);
		animationPlayer.Play("new_animation");
	}

	
	/*
	private void _on_select_1_button_pressed()
	{
		
		// When a given button is pressed, start blinking animation
		// for all other buttons, stop blinking
		animationPlayer.Stop();
		var blink = GetNode<TextureRect>("select2/blink_asset");
		blink.Visible = false; 
		
		animationPlayer = GetNode<AnimationPlayer>("select1/blink");
	}
	*/
}
