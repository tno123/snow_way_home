using Godot;
using System;

public partial class Button : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	private AnimatedSprite2D animation;
	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_area_2d_body_entered(Node2D body)
	{
		animation.Play("main");
		// Replace with function body.
	}
}



