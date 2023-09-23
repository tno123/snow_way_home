using Godot;
using System;

public partial class Avalanche : Node2D
{
	[Export]
	public float Speed = 200.0f;

	public AnimationPlayer AnimationPlayer;

	private Node2D Endpoint;
	private Vector2 startPosition;
	private Vector2 EndPosition;
	private bool atEnd = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		startPosition = Position;
		try {
			Endpoint = GetNode<Node2D>("Endpoint");
		} catch (Exception e) {
			GD.Print("Avalanche.cs: Need Endpoint child node: Error: " + e.Message);
		}
		EndPosition = Endpoint.GlobalPosition;
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!atEnd)
		{
			Position += (new Vector2(-1,0).Rotated(Rotation)) * Speed * (float)delta;
			if (Position.X < EndPosition.X)
			{
				atEnd = true;
				AnimationPlayer.Play("collapse");
			}
		}
	}

	private void _on_area_2d_body_entered(Node2D body)
	{
		if (body is Snowball)
		{
			((Snowball)body).Death();
		}

	}

	private void _on_animation_player_animation_finished(StringName anim_name)
	{
		if (anim_name == "collapse")
		{
			Position = startPosition;
			atEnd = false;
			AnimationPlayer.Play("RESET");
		}
	}
}
