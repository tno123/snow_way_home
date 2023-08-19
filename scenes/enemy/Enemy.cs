using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public Line2D LineOfSight;

	[Export]
	public float MinDistance = 200.0f;

	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	public Snowball Snowball;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		//Add points to the LineOfSight.
		LineOfSight.AddPoint(Position,0);
		Snowball = GetParent().GetNode<Snowball>("Snowball");
		LineOfSight.AddPoint(Snowball.Position,1);
	}

	public override void _PhysicsProcess(double delta)
	{
		UpdateLineOfSight();


		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;


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
		}
		else
		{
			LineOfSight.DefaultColor = new Color(1, 0, 0);
		}

	}
}
