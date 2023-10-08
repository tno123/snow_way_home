using Godot;
using System;

public partial class SnowPower : ProgressBar
{
	//Temp child
	public Sprite2D Sprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Assume child of CanvasLayer
		var snowball = GetParent().GetParent().GetNode<Snowball>("Snowball");
		MaxValue = snowball.Power;
		Value = snowball.Power;
		snowball.Powerup += PowerupCollected;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

	private void PowerupCollected(int value)
	{
		Value += value;
	}
}
