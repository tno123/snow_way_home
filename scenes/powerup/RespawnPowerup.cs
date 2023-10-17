using Godot;
using System;

public partial class RespawnPowerup : Powerup
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PickupSound = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

	private void _on_timer_timeout()
	{
		GetNode<Area2D>("Area2D")
			.GetNode<CollisionShape2D>("CollisionShape2D")
			.CallDeferred("set_disabled", false);
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Visible = true;
		GetNode<Timer>("Timer").Stop();
	}

	protected override void _on_area_2d_body_entered(Node2D body)
	{
		GetNode<Area2D>("Area2D")
			.GetNode<CollisionShape2D>("CollisionShape2D")
			.CallDeferred("set_disabled", true);
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Visible = false;
		GetNode<Timer>("Timer").Start();
		EmitSignal(SignalName.PowerupCollected);
		PickupSound.Play();
	}
}
