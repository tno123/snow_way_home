using Godot;
using System;

public partial class WellTransition : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_area_2d_body_entered(Node2D body)
	{
		if (body is Snowball)
		{
			var transition = GetNode<Transitions>("Transitions");
			transition.Visible=true;
			transition.animationPlayer.Play("fade_out");
		}
	}
}



