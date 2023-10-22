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
	private Snowball snowball;
	private CollisionShape2D collisionShape2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		startPosition = Position;
		snowball = GetParent().GetParent().GetNode<Snowball>("Snowball");
		collisionShape2D = GetNode<Area2D>("Area2D").GetNode<CollisionShape2D>("CollisionShape2D");
		try
		{
			Endpoint = GetNode<Node2D>("Endpoint");
		}
		catch (Exception e)
		{
			GD.Print("Avalanche.cs: Need Endpoint child node: Error: " + e.Message);
		}
		EndPosition = Endpoint.GlobalPosition;
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//if the snowball is close to startPosition, make invisible and non-collidable
		var tooClose = snowball.GlobalPosition.DistanceTo(startPosition) < 100;
		if (tooClose)
		{
			Visible = false;
			collisionShape2D.Disabled = true;
		}
		if (!atEnd && !tooClose)
		{
			Position += (new Vector2(-1, 0).Rotated(Rotation)) * Speed * (float)delta;
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
