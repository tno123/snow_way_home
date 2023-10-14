using Godot;
using System;

public partial class VillagerMob : CharacterBody2D
{
	[Export]
	public float MinDistance = 2000.0f;

	public bool canMove = true;

	[Export]
	public Line2D LineOfSight;

	public const float Speed = 300.0f;
	private Vector2 PreviousVelocity;
	public const float JumpVelocity = -400.0f;

	public Snowball Snowball;
	public AnimationPlayer AnimationPlayer;
	public Sprite2D sprite;

	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Snowball = GetParent().GetParent().GetNode<Snowball>("Snowball");
		if (LineOfSight != null)
		{
			//Add points to the LineOfSight.
			LineOfSight.AddPoint(Position, 0);
			LineOfSight.AddPoint(Snowball.Position, 1);
		}

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
		UpdateLineOfSight();

		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		if (canMove)
		{
			if (Position.X < Snowball.Position.X)
			{
				Position += new Vector2(Speed * (float)delta, 0);
				sprite.FlipH = false;
			}
			else
			{
				Position -= new Vector2(Speed * (float)delta, 0);
				sprite.FlipH = true;
			}
		}

		Velocity = velocity;
		MoveAndSlide();

		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			if (collision.GetCollider() is Snowball)
			{
				//Damage snowball and destroy self
				Snowball.Damage(1);
				if (LineOfSight != null)
					LineOfSight.QueueFree();
			}
		}
	}

	private void UpdateLineOfSight()
	{
		if (LineOfSight == null)
			return;
		LineOfSight.ClearPoints();
		LineOfSight.AddPoint(Position);
		LineOfSight.AddPoint(Snowball.Position);
		// if length of line if shorter than 100, color line green, else color it red
		if (
			LineOfSight.GetPointPosition(0).DistanceTo(LineOfSight.GetPointPosition(1))
			< MinDistance
		)
		{
			LineOfSight.DefaultColor = new Color(0, 1, 0);
		}
		else
		{
			LineOfSight.DefaultColor = new Color(1, 0, 0);
		}
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
