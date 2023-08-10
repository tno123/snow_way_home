using Godot;
using System;

public partial class Key : Sprite2D
{
	private AnimatedSprite2D animation;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		animation.Play("main");
	}
}
