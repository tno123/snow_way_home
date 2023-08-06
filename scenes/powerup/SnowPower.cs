using Godot;
using System;

public partial class SnowPower : ProgressBar
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Assume child of CanvasLayer
		var snowball = GetParent().GetParent().GetNode<Snowball>("Snowball");
		MaxValue = snowball.MaxJumps;
		snowball.PowerupCollected += OnPowerupCollected;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPowerupCollected()
	{
		Value += 1;
	}
}
