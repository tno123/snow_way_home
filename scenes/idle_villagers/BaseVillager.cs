using Godot;
using System;

public partial class BaseVillager : Node2D
{
	private AnimatedSprite2D exclamation;
	public AnimationPlayer AnimationPlayer;
	private Sprite2D sprite;

	public Snowball Snowball;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		exclamation = GetNode<AnimatedSprite2D>("Exclamation");
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		sprite = GetNode<Sprite2D>("Sprite2D");
		Snowball = GetParent().GetNode<Snowball>("Snowball");

		AnimationPlayer.Play("idle");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Position.X < Snowball.Position.X)
			{
				sprite.FlipH = false;
			}
		else
			{
				
				sprite.FlipH = true;
			}
		
	}
	private void _on_area_2d_body_entered(Node2D body)
	{
		if (body is Snowball){
			exclamation.Play("default");
			sprite.FlipH = true;
			AnimationPlayer.Stop();
			
		}
	}
}
