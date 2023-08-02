using Godot;
using System;

public partial class Snowball_TD : Node2D
{
	[Export]
	public int Speed = 400;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;
		if (Input.IsActionPressed("ui_right"))
		{
			velocity.X += 1;
		}
		if (Input.IsActionPressed("ui_left"))
		{
			velocity.X -= 1;
		}
		if (Input.IsActionPressed("ui_down"))
		{
			velocity.Y += 1;
		}
		if (Input.IsActionPressed("ui_up"))
		{
			velocity.Y -= 1;
		}
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			Position += velocity * (float)delta;
			Position = new Vector2(
				x: Mathf.Clamp(Position.X, 0, GetViewportRect().Size.X),
				y: Mathf.Clamp(Position.Y, 0, GetViewportRect().Size.Y)
			);
		}
	}
}
