using Godot;
using System;

public partial class BossHand : Node2D
{
	[Export]
	public bool flipped = false;

	[Signal]
	public delegate void BossDamageEventHandler();
	public string State = "idle";
	public Vector2 Velocity = Vector2.Zero;
	public bool hit = false;
	public AudioStreamPlayer HitSound;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		HitSound = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		if (flipped)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += Velocity * (float)delta;
	}

	private void _on_area_2d_body_entered(Node2D body)
	{
		if (body is Snowball && (State != "smash" || State != "smash_hit"))
		{
			//If snowball enters from above, damage boss
			if (body.GlobalPosition.Y < GlobalPosition.Y)
			{
				EmitSignal(SignalName.BossDamage);
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("take_damage");
				State = "take_damage";
				HitSound.Play();
				return;
			}

			//Else damage snowball
			hit = true;
			((Snowball)body).Damage(1);
		}
		else if (body is StaticBody2D)
		{
			hit = true;
			// Move back a bit
			Position = Position - Velocity.Normalized() * 20;
		}
	}
}
