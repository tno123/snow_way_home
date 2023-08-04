using Godot;
using System;

public partial class Transitions : Control
{
	private TextureRect textureRect;
	private AnimationPlayer animationPlayer;
	
	[Export]
	String scene_switch_anim = "fade_out";
	[Export] 
	PackedScene scene_to_load;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		textureRect = GetNode<TextureRect>("TextureRect");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		textureRect.Visible = false;

	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void SetNextAnimation(string fade_direction){
		
		
		animationPlayer.Queue(fade_direction);
		}
	/*
	public void SetNextAnimation(bool fade_out = true){
		
		
		if(fade_out){
			
			animationPlayer.Queue("fade_out");
		}
		else{
			animationPlayer.Queue("fade_in");
		}
		*/

	
	
	private void _on_animation_player_animation_finished(StringName anim_name)
	{
		GD.Print(scene_to_load);
		if (scene_to_load != null && anim_name == scene_switch_anim){
			GetTree().ChangeSceneToPacked(scene_to_load);
		}
	}	
}
