using Godot;
using System;

public partial class Boss : Node2D
{
	public float damping = 0.1f;
	public float maxVelocity = 300.0f;
	private string rState = "idle";
	private string lState = "idle";
	private bool actionStarted = false;
	private Vector2 attackTargetPosition;
	private BossHand currentHand;
	private Snowball snowball;
	private BossHand leftHand;
	private BossHand rightHand;
	private Timer attackTimer;
	private Polygon2D bounds;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		snowball = GetParent().GetNode<Snowball>("Snowball");
		leftHand = GetNode<BossHand>("BossHand2");
		rightHand = GetNode<BossHand>("BossHand");
		attackTimer = GetNode<Timer>("AttackTimer");

		//Set the initial position of the hands
		leftHand.GlobalPosition = new Vector2(snowball.Position.X - 100, snowball.Position.Y - 100);
		rightHand.GlobalPosition = new Vector2(
			snowball.Position.X + 100,
			snowball.Position.Y - 100
		);

		//Define the bounds of the boss
		bounds = GetParent().GetNode<Polygon2D>("BattleArea");

		//Start the attack timer
		attackTimer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!Geometry2D.IsPointInPolygon(snowball.Position, bounds.Polygon))
		{
			//If snowball is outside the bounds, stop the hands from moving
			leftHand.Velocity = Vector2.Zero;
			rightHand.Velocity = Vector2.Zero;
			return;
		}

		//If timer is finished, select a random attack
		if (attackTimer.TimeLeft <= 0 && rState == "idle" && lState == "idle")
		{
			Random random = new Random();
			int attack = random.Next(0, 2);
			attack = 0;
			if (attack == 0)
			{
				rState = "smash_windup";
				rightHand.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("smash_windup");
			}
			else if (attack == 1)
			{
				lState = "fire_windup";
			}
		}

		UpdateHands((float)delta);
	}

	// Update the position of the hands to follow the snowball and move in a small circle
	private void UpdateHands(float delta)
	{
		//target for hands
		Vector2 lTarget = targetForHand(leftHand, lState);
		Vector2 rTarget = targetForHand(rightHand, rState);

		UpdateHand(leftHand, lTarget, delta, lState);
		UpdateHand(rightHand, rTarget, delta, rState);
	}

	// Update the position of a hand to follow its target and move in a small circle
	private void UpdateHand(BossHand hand, Vector2 target, float delta, string state)
	{
		if (state == "idle")
		{
			// Apply drag to the hand's velocity
			hand.Velocity = hand.Velocity.Lerp(
				(target - hand.GlobalPosition) / (delta * 10),
				damping
			);

			// Clamp the velocity of the hand to the maximum value
			if (hand.Velocity.Length() > maxVelocity)
			{
				hand.Velocity = hand.Velocity.Normalized() * maxVelocity;
			}

			// Update the position of the hand based on its velocity
			hand.GlobalPosition += hand.Velocity * delta;

			// Make the hand move in a small circle
			hand.Position += new Vector2(
				Mathf.Cos(Time.GetTicksMsec() / 500.0f) * 5,
				Mathf.Sin(Time.GetTicksMsec() / 500.0f) * 5
			);
		}
		else if (state == "smash_windup")
		{
			// Apply drag to the hand's velocity
			hand.Velocity = hand.Velocity.Lerp(
				(target - hand.GlobalPosition) / (delta * 50),
				damping
			);

			// Clamp the velocity of the hand to the maximum value
			if (hand.Velocity.Length() > maxVelocity)
			{
				hand.Velocity = hand.Velocity.Normalized() * maxVelocity;
			}

			// Update the position of the hand based on its velocity
			hand.GlobalPosition += hand.Velocity * delta;

			// if the hand is close enough to the target, start the smash attack
			if (hand.GlobalPosition.DistanceTo(target) < 10)
			{
				hand.Velocity = Vector2.Zero;
				hand.GlobalPosition = target;
				rState = "smash";
			}
		}
		else if (state == "smash")
		{
			// Move the hand straight down
			hand.Position += new Vector2(0, 10);
			if (hand.hit)
			{
				hand.hit = false;
				rState = "idle";
				hand.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idle");
				attackTimer.Start();
			}
		}
	}

	private Vector2 targetForHand(BossHand hand, string state)
	{
		if (state == "idle")
		{
			if (hand == leftHand)
			{
				return new Vector2(snowball.Position.X - 100, snowball.Position.Y - 100);
			}
			else if (hand == rightHand)
			{
				return new Vector2(snowball.Position.X + 100, snowball.Position.Y - 100);
			}
		}
		else if (state == "smash_windup")
		{
			if (!actionStarted)
			{
				attackTargetPosition = new Vector2(snowball.Position.X, snowball.Position.Y - 200);
				actionStarted = true;
				return attackTargetPosition;
			}
			else
				return attackTargetPosition;
		}
		return Vector2.Zero;
	}
}
