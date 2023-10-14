using Godot;
using System;

public partial class PermanentBonfire : Bonfire
{
	// Called when the node enters the scene tree for the first time.
	//private Snowball Snowball = null;
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	protected override void _on_area_2d_body_entered(Node2D body)
	{
		base._on_area_2d_body_entered(body);
		GetNode<Timer>("FireTimer").Start();
	}

	private void _on_area_2d_body_exited(Node2D body)
	{
		if (body is Snowball)
		{
			Snowball = null;
		}
	}

	private void _on_fire_timer_timeout()
	{
		if (Snowball != null)
		{
			base.damageSnowball(Snowball);

			GetNode<Timer>("FireTimer").Start();
		}
	}
}
