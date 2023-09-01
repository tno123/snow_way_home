using Godot;
using System;

public partial class SnowEater : CharacterBody2D
{
	[Export]
	public float MinDistance = 200.0f;
	[Export]
	public Line2D LineOfSight;

	public const float Speed = 100.0f;
	public const float JumpVelocity = -400.0f;

	public Snowball Snowball;
	public AnimationPlayer AnimationPlayer;
	public Sprite2D Sprite2D;

	private bool WasOnEdge = false;
	private bool DirRight = true;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private string mode = "idle";

	public override void _Ready()
	{
		Snowball = GetParent().GetParent().GetNode<Snowball>("Snowball");
		if (LineOfSight != null) {
		//Add points to the LineOfSight.
			LineOfSight.AddPoint(Position,0);
			LineOfSight.AddPoint(Snowball.Position,1);
		}

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

		mode = Position.DistanceTo(Snowball.Position) < MinDistance ? "walk" : "idle";

		//if mode is walk, walk towards snowball. Take care to flip the sprite if the snowball is to the left.
		if (mode == "walk" && NotOnEdge())
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
			mode="idle";
			velocity.X = 0;
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
				QueueFree();
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
		if (LineOfSight.GetPointPosition(0).DistanceTo(LineOfSight.GetPointPosition(1)) < MinDistance)
		{
			LineOfSight.DefaultColor = new Color(0, 1, 0);
		}
		else
		{
			LineOfSight.DefaultColor = new Color(1, 0, 0);
		}

	}

	private bool NotOnEdge() 
	{
		// Use raycast to check if there is a floor in front of the enemy
		var spaceState = GetWorld2D().DirectSpaceState;
		var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, GlobalPosition + new Vector2(Speed, Speed));
		var result = spaceState.IntersectRay(query);
		Variant value;
		if (result.TryGetValue("normal",out value))
		{
			if ((Vector2)value == Vector2.Up || (Vector2)value == Vector2.Down)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}

}
