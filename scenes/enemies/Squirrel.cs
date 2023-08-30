using Godot;
using System;

public partial class Squirrel : Node2D
{
	[Export]
	public float ThrowingDistance = 400.0f;
	[Export]
	public float ThrowTime = 2.5f;
	public Snowball snowball;
	public Timer timer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Assume squirrels are organized in basic parent node
		snowball = GetParent().GetParent().GetNode<Snowball>("Snowball");
		timer = GetNode<Timer>("Timer");
		timer.WaitTime = ThrowTime;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//if snowball is close enough to squirrel, then squirrel will throw projectile
		if (snowball.Position.DistanceTo(this.Position) < ThrowingDistance && timer.IsStopped())
		{
			timer.Start();
			var acorn = (Acorn)GD.Load<PackedScene>("res://scenes/enemies/acorn/Acorn.tscn").Instantiate();
			acorn.Direction = (snowball.Position - this.Position).Normalized();
			AddChild(acorn);
		}
	}
}
