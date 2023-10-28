using Godot;
using System;

public partial class Snowball : CharacterBody2D
{
	[Export]
	public int Power = 3;

	[Export]
	public float IceSpeed = 5.0f;

	[Export]
	public float CoyoteTime = 0.1f;

	[Export]
	public float NextJumpTime = 0.1f;

	[Export]
	public const float JumpVelocity = -350.0f;

	[Export]
	public float DarkenTime = 5.0f;

	[Signal]
	public delegate void PowerupEventHandler(int value);

	[Signal]
	public delegate void DarkenScreenEventHandler(bool value);

	public const float Speed = 200.0f;
	public const float BounceVelocity = -750.0f;
	public const float BoostVelocity = 2000.0f;
	public float FallSpeed = 1.0f;
	public bool CanJump = true;
	public TileMap tileMap;
	public StaticBody2D staticBody2D;
	public bool CurrentIce = false;
	public Vector2 Checkpoint = Vector2.Zero;
	public Snowbank Snowbank;
	public AudioStreamPlayer JumpSound;
	public AudioStreamPlayer HurtSound;

	private int MaxPower = 3;
	private Timer CoyoteJumpTimer;
	private Timer NextJumpTimer;
	private Timer HidingTimer;
	private Vector2 velocity = Vector2.Zero;
	private bool WasOnFloor = false;
	private bool IsOnIce = false;
	private float lastVelocityX = 0;
	private AnimatedSprite2D WalkDustAnimation;
	private Sprite2D sprite;

	private AnimatedSprite2D BoostSmokeAnimation;
	private AnimatedSprite2D DeathAnimation;

	private AnimatedSprite2D JumpLandAnim;
	private AnimatedSprite2D JumpAnim;
	private AnimatedSprite2D SnowDiveAnim;
	private bool canMove = true;

	private Timer DamageBoostTimer;
	private Timer BlinkTimer;
	private Timer MovementBoostTimer;
	private Timer DeathTimer;
	private bool isDamaged = false;

	private Vector2 PreviousVelocity = Vector2.Zero;

	private bool isBoosting = false;
	private bool hasDived = false;

	private Vector2 originalScale;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		originalScale = Scale;
		WalkDustAnimation = GetNode<AnimatedSprite2D>("WalkDustAnimation");

		BoostSmokeAnimation = GetNode<AnimatedSprite2D>("BoostSmokeAnimation");
		DeathAnimation = GetNode<AnimatedSprite2D>("DeathAnimation");
		SnowDiveAnim = GetNode<AnimatedSprite2D>("SnowDiveAnim");

		JumpAnim = GetNode<AnimatedSprite2D>("JumpAnim");
		JumpLandAnim = GetNode<AnimatedSprite2D>("JumpLandAnim");

		JumpSound = GetNode<AudioStreamPlayer>("JumpSound");
		HurtSound = GetNode<AudioStreamPlayer>("HurtSound");

		CoyoteJumpTimer = GetNode<Timer>("CoyoteJumpTimer");
		NextJumpTimer = GetNode<Timer>("NextJumpTimer");
		HidingTimer = GetNode<Timer>("HidingTimer");
		MovementBoostTimer = GetNode<Timer>("MovementBoostTimer");
		DeathTimer = GetNode<Timer>("DeathTimer");
		CoyoteJumpTimer.WaitTime = CoyoteTime;
		NextJumpTimer.WaitTime = NextJumpTime;
		HidingTimer.WaitTime = DarkenTime;
		Checkpoint = Position;

		//Groups are good thumbsup emoji
		SubscribeToSignals("Powerups", "Powerups", "PowerupCollected", "OnPowerup");
		SubscribeToSignals("MapObjects", "Puddles", "PuddleEntered", "OnIced");
		SubscribeToSignals("MapObjects", "BouncePads", "Bounce", "_on_bounce_pad_bounce");
		SubscribeToSignals("MapObjects", "Checkpoints", "SetCheckpoint", "OnCheckpoint");

		Snowbank = GetNodeOrNull<Snowbank>("Snowbank");

		DamageBoostTimer = GetNode<Timer>("DamageBoostTimer");
		BlinkTimer = GetNode<Timer>("BlinkTimer");
	}

	public override void _PhysicsProcess(double delta)
	{
		tileMap = GetParent().GetNodeOrNull<TileMap>("TileMap");
		IsOnIce = false;
		WasOnFloor = IsOnFloor();

		velocity = Velocity;

		HandleSnowbank();

		// Add the gravity.
		ApplyGravity((float)delta);

		// Handle Jump, Coyote Jump, and Next Jump

		//test test test

		if (canMove)
		{
			HandleJump();
			HandleBoost();
			if (IsOnFloor() && PreviousVelocity.Y > 800)
			{
				Damage(1);
			}
			if (
				GetNode<Timer>("InvulnerabilityTimer").IsStopped()
				&& GetNode<AnimatedSprite2D>("IceSprite").Animation == "breaking"
			)
			{
				GetNode<AnimatedSprite2D>("IceSprite").Visible = false;
			}

			// Get the input direction and handle the movement/deceleration.
			// As good practice, you should replace UI actions with custom gameplay actions.
			Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			if (direction.X != 0 && !isBoosting)
			{
				//if (!HandleIceTile())
				velocity.X = direction.X * Speed;
				//else
				//	velocity.X = direction.X * PreviousVelocity.X / IceSpeed;

				if (IsOnFloor())
				{
					WalkDustAnimation.Play("default");
				}
				//else{animation.Stop();}

				if (direction.X < 0)
				{
					Vector2 newOffset = sprite.Offset;
					newOffset.X = 20;
					BoostSmokeAnimation.Offset = WalkDustAnimation.Offset = newOffset;
					BoostSmokeAnimation.FlipH = false;
					sprite.FlipH = WalkDustAnimation.FlipH = false;
				}
				if (direction.X > 0)
				{
					Vector2 newOffset = sprite.Offset;
					newOffset.X = -20;
					BoostSmokeAnimation.Offset = WalkDustAnimation.Offset = newOffset;
					BoostSmokeAnimation.FlipH = true;
					sprite.FlipH = WalkDustAnimation.FlipH = true;
				}
			}
			else
			{
				WalkDustAnimation.Stop();
				//TODO: Need to handle correct ice physics
				IsOnIce = HandleIceTile();
				//IsOnIce=false;
				//Friction
				if (IsOnIce)
					velocity.X = Mathf.Lerp(Velocity.X, 0, 0.01f);
				else
					velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
				//lastVelocityX = velocity.X;
			}

			Velocity = velocity;
			PreviousVelocity = velocity;

			MoveAndSlide();

			// Handle coyote jump timer.
			if (!IsOnFloor() && WasOnFloor && velocity.Y >= 0)
			{
				CoyoteJumpTimer.Start();
				WasOnFloor = false;
			}
			else if (IsOnFloor())
			{
				CoyoteJumpTimer.Stop();
				NextJumpTimer.Stop();
				//Power = 1;
			}
		}
	}

	void HandleJump()
	{
		// Handle Jump, Coyote Jump, and Next Jump
		if (
			Input.IsActionJustPressed("ui_accept") && IsOnFloor()
			|| !IsOnFloor() && Input.IsActionPressed("ui_accept") && CoyoteJumpTimer.TimeLeft > 0
		)
		{
			//NextJumpTimer.Start();

			velocity.Y = JumpVelocity;
			CoyoteJumpTimer.Stop();
			NextJumpTimer.Start();
			JumpSound.Play();
		}

		if (
			Input.IsActionJustPressed("ui_accept")
			&& !IsOnFloor()
			&& NextJumpTimer.IsStopped()
			&& Power > 0 
			&& CanJump
		)
		{
			JumpAnim.Play("main");
			NextJumpTimer.Start();
			velocity.Y = JumpVelocity;
			EmitSignal(SignalName.Powerup, -1);
			Power--;
			JumpSound.Play();
		}
		if (PreviousVelocity.Y > 400 && IsOnFloor())
		{
			JumpLandAnim.Play("main");
		}
	}

	private void HandleBoost()
	{
		// Check if the X button is just pressed
		if (Input.IsActionJustPressed("boost") && !isBoosting)
		{
			MovementBoostTimer.Start();
			StartBoost();
		}
	}

	private void StartBoost()
	{
		isBoosting = true;

		BoostSmokeAnimation.Play("default");
		if (sprite.FlipH) // Facing right
		{
			velocity.X += BoostVelocity;
			Velocity = velocity;
			MoveAndSlide();
		}
		else // Facing left
		{
			velocity.X -= BoostVelocity;
			Velocity = velocity;
			MoveAndSlide();
		}
	}

	void ApplyGravity(float delta)
	{
		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * delta * FallSpeed;
		}
	}

	bool HandleIceTile()
	{
		// Pressing "down" negates ice physics
		var retVal = false;
		try
		{
			var collider = GetLastSlideCollision().GetCollider();
			var colliderRid = GetLastSlideCollision().GetColliderRid();
			if (colliderRid != null) //Todo: Check if this is the correct way to check for null
			{
				//check if standing on staticbody
				if (collider.GetClass() == "StaticBody2D")
					return false;

				var tileData = tileMap.GetCellTileData(0, tileMap.GetCoordsForBodyRid(colliderRid));
				if ((bool)tileData.GetCustomData("Ice") == true && IsOnFloor())
				{
					retVal = true;
				}
			}
		}
		catch (NullReferenceException)
		{
			// Do nothing
		}
		return retVal;
	}

	void HandleSnowbank()
	{
		if (Snowbank != null)
		{
			if (Input.IsActionPressed("ui_down"))
			{
				var allAvalanches = GetTree().GetNodesInGroup("avalanches");
				for (int i = 0; i < allAvalanches.Count; i++)
				{
					var avalanche = (Avalanche)allAvalanches[i];
					avalanche
						.GetNode<Area2D>("Area2D")
						.GetNode<CollisionShape2D>("CollisionShape2D")
						.Disabled = true;
				}
				GetNode<Sprite2D>("Sprite2D").Visible = false;
				canMove = false;
				if (!hasDived)
				{
					SnowDiveAnim.Rotation = Snowbank.Rotation;
					SnowDiveAnim.Visible = true;
					SnowDiveAnim.Play("main");
					hasDived = true;
				}
				EmitSignal(SignalName.DarkenScreen, true);
				if (HidingTimer.IsStopped())
					HidingTimer.Start();
			}
			else
			{
				var allAvalanches = GetTree().GetNodesInGroup("avalanches");
				for (int i = 0; i < allAvalanches.Count; i++)
				{
					var avalanche = (Avalanche)allAvalanches[i];
					avalanche
						.GetNode<Area2D>("Area2D")
						.GetNode<CollisionShape2D>("CollisionShape2D")
						.Disabled = false;
				}
				GetNode<Sprite2D>("Sprite2D").Visible = true;
				canMove = true;
				SnowDiveAnim.Visible = false;
				hasDived = false;
				EmitSignal(SignalName.DarkenScreen, false);
				HidingTimer.Stop();
			}
		}
	}

	public void Damage(int damage)
	{
		if (
			!isDamaged
			&& GetNode<Timer>("InvulnerabilityTimer").IsStopped()
			&& DamageBoostTimer.IsStopped()
			&& DeathTimer.IsStopped()
			&& !CurrentIce
		)
		{
			isDamaged = true;
			Power -= damage;
			EmitSignal(SignalName.Powerup, -damage);
			if (Power <= 0)
				Death();
			DamageBoostTimer.Start();
			BlinkTimer.Start();
			HurtSound.Play();
		}
		if (CurrentIce)
		{
			CurrentIce = false;
			OnIced(false);
			HurtSound.Play();
		}
	}

	public void Death()
	{
		sprite.Visible = false;

		DeathAnimation.Play("default");
		Scale = originalScale;
		if (!BlinkTimer.IsStopped())
			BlinkTimer.Stop();
		if (DeathTimer.IsStopped())
		{
			DeathTimer.Start();
		}
	}

	public void SetIce(bool ice)
	{
		CurrentIce = ice;
		OnIced(ice);
	}

	public void StopMovement()
	{
		canMove = false;
	}

	public void StartMovement()
	{
		canMove = true;
	}

	private void SubscribeToSignals(
		string parentNodeName,
		string groupName,
		string signalName,
		string callback
	)
	{
		var parentNode = GetParent().GetNodeOrNull<Node>(parentNodeName);
		if (parentNode != null)
		{
			var nodes = GetTree().GetNodesInGroup(groupName);
			for (int i = 0; i < nodes.Count; i++)
			{
				var node = nodes[i];
				if (node.HasSignal(signalName))
				{
					var callable = new Callable(this, callback);
					node.Connect(signalName, callable);
				}
			}
		}
	}

	private void OnPowerup()
	{
		if (Power < MaxPower)
		{
			EmitSignal(SignalName.Powerup, 1);
			Power++;
		}
	}

	private void OnIced(bool ice)
	{
		CurrentIce = ice;
		var sprite = GetNode<AnimatedSprite2D>("IceSprite");
		sprite.Visible = true;

		if (ice)
			sprite.Play("iced");
		else
		{
			sprite.Play("breaking");
			GetNode<Timer>("InvulnerabilityTimer").Start();
		}
	}

	private void OnCheckpoint()
	{
		Checkpoint = Position;
	}

	private void _on_bounce_pad_bounce()
	{
		velocity.Y = BounceVelocity;
		Velocity = velocity;
		MoveAndSlide();
	}

	private void _on_animated_sprite_2d_animation_finished()
	{
		/* let the animation play out after a jump
		but stop playing when in air */
		if (!IsOnFloor())
		{
			WalkDustAnimation.Stop();
		}
	}

	private void _on_hiding_timer_timeout()
	{
		EmitSignal(SignalName.DarkenScreen, false);
		Snowbank = null;
		Visible = true;
		canMove = true;
		Death();
	}

	private void _on_damage_boost_timer_timeout()
	{
		isDamaged = false;
		BlinkTimer.Stop();
		Visible = true; // Ensure player is visible at the end of damage boost
	}

	private void _on_blink_timer_timeout()
	{
		Visible = !Visible; // Toggle visibility
		if (!DamageBoostTimer.IsStopped())
		{
			BlinkTimer.Start();
		}
	}

	private void _on_movement_boost_timer_timeout()
	{
		isBoosting = false;
	}

	private void _on_death_timer_timeout()
	{
		DeathAnimation.Stop();
		sprite.Visible = true;

		Position = Checkpoint;
		Power = MaxPower;
		EmitSignal(SignalName.Powerup, MaxPower);
	}
}
