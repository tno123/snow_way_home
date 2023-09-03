using Godot;
using System;

public partial class StartIceDestroy : Node
{
	//Has to be child of Snowball
	public Snowball snowball;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		snowball = (Snowball)GetParent();
		snowball.SetIce(true);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_ice_destroy_area_body_entered(Node2D body)
	{
		snowball.FallSpeed = 1.0f;
		snowball.Damage(1);
		QueueFree();
	}
}

