using Godot;
using System;

public partial class Acorn : Area2D
{
	public Sprite2D Sprite;
	public Vector2 Direction;
	private float speed = 100.0f;
	private float rotSpeed = 10.0f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Sprite = GetNode<Sprite2D>("Sprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Sprite.Rotate((float)delta * rotSpeed);
		Position += Direction * (float)delta * speed;
		if (Position.Y > 1000.0f)
		{
			QueueFree();
		}
	}

	private void _on_body_entered(Node2D body)
	{
		if (body is Snowball)
		{
			((Snowball)body).Damage(1);
		}
		QueueFree();
	}
}
