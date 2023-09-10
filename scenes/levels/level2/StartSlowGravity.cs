using Godot;
using System;

public partial class StartSlowGravity : Node
{
	//Has to be child of Snowball
	public Snowball snowball;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		snowball = (Snowball)GetParent();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_slow_gravity_area_body_entered(Node2D body)
	{
		snowball.FallSpeed = 0.05f;
		//snowball.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("slow");
		QueueFree();
	}
}
