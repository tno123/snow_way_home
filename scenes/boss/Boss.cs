using Godot;
using System;

public partial class Boss : Node2D
{
	[Signal]
	public delegate void SmashHitEventHandler();

	[Export]
	public PackedScene Bullet;

	[Export]
	public float Health = 10.0f;
	public float damping = 0.1f;
	public float maxVelocity = 300.0f;
	private bool actionStarted = false;
	private Vector2 attackTargetPosition;
	private BossHand currentHand;
	private Snowball snowball;
	private BossHand leftHand;
	private BossHand rightHand;
	private Timer attackTimer;
	private Timer waitTimer;
	private Polygon2D bounds;
	private ProgressBar bossHealthProgressBar;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bossHealthProgressBar = GetParent()
			.GetNode<CanvasLayer>("UI")
			.GetNode<ProgressBar>("BossHealth");
		snowball = GetParent().GetNode<Snowball>("Snowball");
		leftHand = GetNode<BossHand>("BossHand2");
		rightHand = GetNode<BossHand>("BossHand");
		attackTimer = GetNode<Timer>("AttackTimer");
		//waitTimer used for smash and fire attacks
		waitTimer = GetNode<Timer>("WaitTimer");

		//Set the initial position of the hands
		leftHand.GlobalPosition = new Vector2(snowball.Position.X - 100, snowball.Position.Y - 100);
		rightHand.GlobalPosition = new Vector2(
			snowball.Position.X + 100,
			snowball.Position.Y - 100
		);

		//Subscrive to the BossDamage signal from the hands
		leftHand.BossDamage += OnBossDamage;
		rightHand.BossDamage += OnBossDamage;

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
		if (attackTimer.TimeLeft <= 0 && rightHand.State == "idle" && leftHand.State == "idle")
		{
			Random random = new Random();
			int attack = random.Next(0, 2);
			if (attack == 0)
			{
				rightHand.State = "smash_windup";
				rightHand.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("smash_windup");
			}
			else if (attack == 1)
			{
				leftHand.State = "fire_windup";
				leftHand.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("fire_windup");
				waitTimer.Start();
			}
		}

		UpdateHands((float)delta);
	}

	// Update the position of the hands to follow the snowball and move in a small circle
	private void UpdateHands(float delta)
	{
		//target for hands
		Vector2 lTarget = targetForHand(leftHand, leftHand.State);
		Vector2 rTarget = targetForHand(rightHand, rightHand.State);

		UpdateHand(leftHand, lTarget, delta, leftHand.State);
		UpdateHand(rightHand, rTarget, delta, rightHand.State);
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
				rightHand.State = "smash";
			}
		}
		else if (state == "smash")
		{
			// Move the hand straight down
			hand.Velocity = new Vector2(0, 400);
			if (hand.hit)
			{
				hand.hit = false;
				rightHand.State = "smash_hit";
				waitTimer.Start();

				//send signal to camera to shake
				EmitSignal(SignalName.SmashHit);
			}
		}
		else if (state == "smash_hit")
		{
			//stay in position for a small amount of time
			if (!waitTimer.IsStopped())
			{
				hand.Velocity = Vector2.Zero;
			}
			else
			{
				actionStarted = false;
				rightHand.State = "idle";
				hand.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idle");
				attackTimer.Start();
			}
		}
		else if (state == "fire_windup")
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

			// Make the hand move up and down slightly
			hand.Position += new Vector2(
				Vector2.Zero.X,
				Mathf.Sin(Time.GetTicksMsec() / 500.0f) * 3
			);

			if (hand.GlobalPosition.DistanceTo(target) < 10 && waitTimer.IsStopped())
			{
				hand.Velocity = Vector2.Zero;
				hand.GlobalPosition = target;
				leftHand.State = "fire";
			}
		}
		else if (state == "fire")
		{
			attackTimer.Start();
			hand.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idle");
			Fire(hand);
			leftHand.State = "idle";
			hand.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idle");
			attackTimer.Start();
		}
		else if (state == "take_damage")
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

			//if close enough to target, change state to idle
			if (hand.GlobalPosition.DistanceTo(target) < 30 && waitTimer.IsStopped())
			{
				hand.Velocity = Vector2.Zero;
				hand.GlobalPosition = target;
				hand.State = "idle";
				hand.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idle");
			}
		}
	}

	private Vector2 targetForHand(BossHand hand, string state)
	{
		if (state == "idle" || state == "take_damage")
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
		else if (state == "fire_windup")
		{
			return new Vector2(snowball.Position.X - 200, snowball.Position.Y - 20);
		}
		return Vector2.Zero;
	}

	void Fire(BossHand hand)
	{
		//fire a projectile
		var bullet = (BossBullet)Bullet.Instantiate();
		AddChild(bullet);
		if (hand == leftHand)
		{
			bullet.Right = true;
			bullet.GlobalPosition = hand.GlobalPosition + new Vector2(10, 0);
		}
		else
		{
			bullet.Right = false;
			bullet.GlobalPosition = hand.GlobalPosition + new Vector2(-10, 0);
		}
	}

	void OnBossDamage()
	{
		Health -= 1;
		bossHealthProgressBar.Value -= bossHealthProgressBar.Step;
		waitTimer.WaitTime = 1.0f;
		waitTimer.Start();
		if (Health <= 0)
		{
			//boss defeated
			bossHealthProgressBar.QueueFree();
			QueueFree();
		}
	}
}
