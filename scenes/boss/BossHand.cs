using Godot;
using System;

public partial class BossHand : Node2D
{
	[Export]
	public bool flipped = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (flipped)
		{
			GetNode<Sprite2D>("Sprite2D").FlipH = true;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }
}
