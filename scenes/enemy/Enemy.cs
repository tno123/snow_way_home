using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public Line2D LineOfSight;

	[Export]
	public float MinDistance = 200.0f;

	public const float Speed = 100.0f;
	public const float JumpVelocity = -400.0f;

	public Snowball Snowball;
	public AnimationPlayer AnimationPlayer;
	public Sprite2D Sprite2D;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private string mode = "idle";

	public override void _Ready()
	{
		//Add points to the LineOfSight.
		LineOfSight.AddPoint(Position,0);
		Snowball = GetParent().GetNode<Snowball>("Snowball");
		LineOfSight.AddPoint(Snowball.Position,1);

		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		Sprite2D = GetNode<Sprite2D>("Sprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		UpdateLineOfSight();

		AnimationPlayer.Play(mode);

		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		//if mode is walk, walk towards snowball. Take care to flip the sprite if the snowball is to the left.
		if (mode == "walk")
		{
			if (Position.X < Snowball.Position.X)
			{
				velocity.X = Speed;
				Sprite2D.FlipH = false;
			}
			else
			{
				velocity.X = -Speed;
				Sprite2D.FlipH = true;
			}
		}
		else
		{
			velocity.X = 0;
		}


		/*
		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		*/

		Velocity = velocity;
		MoveAndSlide();
	}

	private void UpdateLineOfSight()
	{
		LineOfSight.ClearPoints();
		LineOfSight.AddPoint(Position);
		LineOfSight.AddPoint(Snowball.Position);
		// if length of line if shorter than 100, color line green, else color it red
		if (LineOfSight.GetPointPosition(0).DistanceTo(LineOfSight.GetPointPosition(1)) < MinDistance)
		{
			LineOfSight.DefaultColor = new Color(0, 1, 0);
			mode = "walk";
		}
		else
		{
			LineOfSight.DefaultColor = new Color(1, 0, 0);
			mode = "idle";
		}

	}
}
