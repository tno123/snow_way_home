using Godot;
using System;

public partial class VillagerMob : CharacterBody2D
{
	[Export]
	public float MinDistance = 2000.0f;

	public bool canMove = true;

	public const float Speed = 300.0f;
	public const float MaxSpeed = 400.0f;
	private Vector2 PreviousVelocity;
	public const float JumpVelocity = -400.0f;

	public Snowball Snowball;
	public AnimationPlayer AnimationPlayer;
	public Sprite2D sprite;

	public Vector2 StartPosition;

	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Snowball = GetParent().GetParent().GetNode<Snowball>("Snowball");
		StartPosition = Position;

		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		sprite = GetNode<Sprite2D>("Sprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (canMove)
		{
			AnimationPlayer.Play("walk");
			Rotation += .3f * (3.14f / 180.0f);
		}

		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		if (canMove)
		{
			if (Position.X < Snowball.Position.X && Velocity.X < Speed)
			{
				velocity += new Vector2(Speed * (float)delta, 0);
				sprite.FlipH = false;
			}
			else
			{
				velocity -= new Vector2(Speed * (float)delta, 0);
				sprite.FlipH = true;
			}
		}

		if (velocity.Length() > MaxSpeed)
			velocity = velocity.Normalized() * MaxSpeed;

		Velocity = velocity;
		MoveAndSlide();

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			if (collision.GetCollider() is Snowball)
			{
				//Damage snowball and destroy self
				Snowball.Damage(1);
				if (Snowball.Power <= 0)
				{
					Position = StartPosition;
					StopMovement();
				}
			}
		}
		if (!canMove && Snowball.Power > 0)
			StartMovement();
	}

	public void StopMovement()
	{
		AnimationPlayer.Stop();
		canMove = false;
		//PreviousVelocity = Velocity;
		//Velocity = new Vector2(0, 0);
	}

	public void StartMovement()
	{
		canMove = true;
		//Velocity = PreviousVelocity;
	}
}
