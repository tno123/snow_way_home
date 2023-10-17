using Godot;
using System;

public partial class Powerup : Node2D
{
	[Signal]
	public delegate void PowerupCollectedEventHandler();
	public AudioStreamPlayer PickupSound;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PickupSound = GetNode<AudioStreamPlayer>("PickupSound");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

	protected virtual void _on_area_2d_body_entered(Node2D body)
	{
		QueueFree();
		EmitSignal(SignalName.PowerupCollected);
		PickupSound.Play();
	}
}
