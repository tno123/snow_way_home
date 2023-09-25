using Godot;
using System.Collections.Generic;

public partial class BonfireRun : Node2D
{
	[Export]
	private int gapWidth = 1; // Width of the gap
	private int gapPosition = 0; // Current gap start position
	private List<Bonfire> bonfires = new List<Bonfire>();

	private Timer gapTimer;

	[Export]
	private bool flip = false;

	public override void _Ready()
	{
		// Get all bonfire children and add them to the list

		gapTimer = GetNode<Timer>("GapTimer");
		gapTimer.Start();

		foreach (Node child in GetChildren())
		{
			if (child is Bonfire)
			{
				bonfires.Add(child as Bonfire);
			}
		}
		if (flip)
		{
			bonfires.Reverse();
		}
		UpdateBonfires();
	}

	private void UpdateBonfires()
	{
		for (int i = 0; i < bonfires.Count; i++)
		{
			if ((i >= gapPosition) && (i < gapPosition + gapWidth))
			{
				bonfires[i].Visible = false; // Turn off the fire within the gap
				bonfires[i]
					.GetNode<Area2D>("Area2D")
					.GetNode<CollisionShape2D>("CollisionShape2D")
					.Disabled = true;
			}
			else
			{
				bonfires[i].Visible = true; // Otherwise, light it
				bonfires[i]
					.GetNode<Area2D>("Area2D")
					.GetNode<CollisionShape2D>("CollisionShape2D")
					.Disabled = false;
			}
		}
	}

	/* public override void _Process(double delta)
	 {
		 gapPosition = (gapPosition + 1) % (bonfires.Count - gapWidth + 1); // Move the gap
		 UpdateBonfires();
	 } */

	private void _on_gap_timer_timeout()
	{
		gapPosition = (gapPosition + 1) % (bonfires.Count - gapWidth + 1); // Move the gap
		//gapPosition = (gapPosition + 1) % (bonfires.Count - gapWidth + 1); // Move the gap
		UpdateBonfires();
	}
}
