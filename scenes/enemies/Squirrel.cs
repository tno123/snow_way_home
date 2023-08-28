using Godot;
using System;

public partial class Squirrel : Node2D
{
	public Snowball snowball;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Assume squirrels are organized in basic parent node
		snowball = GetParent().GetParent().GetNode<Snowball>("Snowball");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//if snowball is close enough to squirrel, then squirrel will throw projectile
		if (snowball.Position.DistanceTo(this.Position) < 100)
		{
			//throw projectile
			//create projectile
			//add projectile to scene
			//set projectile's position to squirrel's position
			GD.Print("asdasd");
		}
	}
}
