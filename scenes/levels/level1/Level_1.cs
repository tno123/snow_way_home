using Godot;
using System;


public partial class Level_1 : Node
{
	private AnimatedSprite2D _animatedSprite;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("res://utils/Transitions.tscn/Area2D/AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_tree_entered()
	{
		_animatedSprite.play("sphere_transition");
	}
}



