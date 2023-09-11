using Godot;
using System;

public partial class Villager : CharacterBody2D
{
	[Export]
	public float MinDistance = 2000.0f;
	[Export]
	public Line2D LineOfSight;

	public const float Speed = 150.0f;
	public const float JumpVelocity = -400.0f;

	public Snowball Snowball;
	public AnimationPlayer AnimationPlayer;
	public Sprite2D Sprite2D;

	private string mode = "walk";
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	
	
	/*
	Villagers som er oppå en annen villager beveger seg fortere,
	tror de "arver" speed til villager under seg

	TODO: 
	- Villagers skal skade snøball når de kolliderer
	- Lage flere typer villagers
	- 
	*/
	// Called when the node enters the scene tree for the first time.
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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
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

		if (Position.DistanceTo(Snowball.Position) < MinDistance)
		{
			if (Position.X < Snowball.Position.X)
			{
				Sprite2D.FlipH = false;
			}
			else
			{
				Sprite2D.FlipH = true;
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
		var direction = Sprite2D.FlipH ? -Speed : Speed;
		var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, GlobalPosition + new Vector2(direction, Speed));
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
