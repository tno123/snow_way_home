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

	[Signal]
	public delegate void PowerupEventHandler(int value);

	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public const float BounceVelocity = -750.0f;
	public const float BoostVelocity = 2000.0f;
	public TileMap tileMap;
	public StaticBody2D staticBody2D;
	private int MaxPower = 3;
	private Timer CoyoteJumpTimer;
	private Timer NextJumpTimer;
	private Vector2 velocity = Vector2.Zero;
	private bool WasOnFloor = false;
	private bool IsOnIce = false;
	private float lastVelocityX = 0;
	private AnimatedSprite2D animation;
	private Sprite2D sprite;
	
	private AnimatedSprite2D BoostAnimLeft;
	private AnimatedSprite2D BoostAnimRight;
	private AnimatedSprite2D BoostChargingAnim;
	private AnimatedSprite2D JumpLandAnim;
	private AnimatedSprite2D JumpAnim;
	
	private bool isBoosting = false;
	
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		BoostAnimLeft = GetNode<AnimatedSprite2D>("BoostAnimationLeft");
		BoostAnimRight = GetNode<AnimatedSprite2D>("BoostAnimationRight");
		BoostChargingAnim = GetNode<AnimatedSprite2D>("BoostChargingAnim");
		
		JumpAnim = GetNode<AnimatedSprite2D>("JumpAnim");
		JumpLandAnim = GetNode<AnimatedSprite2D>("JumpLandAnim");
		
		
		CoyoteJumpTimer = GetNode<Timer>("CoyoteJumpTimer");
		NextJumpTimer = GetNode<Timer>("NextJumpTimer");
		CoyoteJumpTimer.WaitTime = CoyoteTime;
		NextJumpTimer.WaitTime = NextJumpTime;

		//TODO: This is a hacky way to get the powerups, but it works for now (REFACTOR)
		var numPowerups = GetParent().GetNode<Node>("Powerups").GetChildCount();
		for (int i = 0; i < numPowerups; i++)
		{
			var powerup = (Powerup)GetParent().GetNode("Powerups").GetChild(i);
			powerup.PowerupCollected += OnPowerup;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		
		tileMap = GetParent().GetNode<TileMap>("TileMap");
		IsOnIce = false;
		WasOnFloor = IsOnFloor();

		velocity = Velocity;
		

		// Add the gravity.
		ApplyGravity((float)delta);

		// Handle Jump, Coyote Jump, and Next Jump
		HandleJump();
		HandleBoost();

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction.X != 0)
		{
			if (!HandleIceTile())
				
			velocity.X = direction.X * Speed;
			if (IsOnFloor()){animation.Play("move");}
			//else{animation.Stop();}
			
			if (direction.X < 0){
				Vector2 newOffset = animation.Offset;
				newOffset.X = 0;
				animation.Offset = newOffset;				
				sprite.FlipH = animation.FlipH = false;
			}
			if (direction.X > 0){
				Vector2 newOffset = animation.Offset;
				newOffset.X = -15;
				animation.Offset = newOffset;
				sprite.FlipH = animation.FlipH = true;
			}
			
		}
		else
		{
			animation.Stop();
			//TODO: Need to handle correct ice physics
			IsOnIce = HandleIceTile();
			//Friction
			velocity.X = Mathf.MoveToward(Velocity.X, 0, IsOnIce? IceSpeed : Speed);
			//lastVelocityX = velocity.X;
		}

		Velocity = velocity;
		
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

	void HandleJump() {
		bool jumped = false;
		// Handle Jump, Coyote Jump, and Next Jump
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor() ||
			!IsOnFloor() && Input.IsActionPressed("ui_accept") && CoyoteJumpTimer.TimeLeft > 0)
		{
			//NextJumpTimer.Start();
			JumpAnim.Play("main");
			velocity.Y = JumpVelocity;
			CoyoteJumpTimer.Stop();
			NextJumpTimer.Start();
			jumped = true;
		}
		
		if (Input.IsActionJustPressed("ui_accept") && !IsOnFloor() && NextJumpTimer.IsStopped() && Power > 0)
		{
			NextJumpTimer.Start();
			velocity.Y = JumpVelocity;
			jumped = true;
			EmitSignal(SignalName.Powerup, -1);
			Power--;
		}
		if (WasOnFloor && IsOnFloor()) {
			JumpLandAnim.Play("main");
			JumpAnim.Stop();
		}
	}
	
	private void HandleBoost()
	{
		// Check if the X button is just pressed
		if (Input.IsActionJustPressed("boost") && !isBoosting)
		{
			StartBoost();
		}
		 // Check if the X button (or your defined action) is being held down
		if (Input.IsActionPressed("boost") && isBoosting)
		{
			BoostChargingAnim.Play("boost_charging");
		}

		// Check if the X button is just released
		if (Input.IsActionJustReleased("boost") && isBoosting)
		{
			EndBoost();
		}
	}
	public bool IsBoosting
	{
		get { return isBoosting; }
	}
	
	private void StartBoost()
	{
		isBoosting = true;

		// Slight scale change to help indicate boost
		sprite.Scale = new Vector2(0.9f, 0.9f);

		BoostAnimLeft.Play("left_boost");
		BoostAnimRight.Play("right_boost");
	}
	
	private void EndBoost()
{
	isBoosting = false;
	BoostChargingAnim.Stop();

	// Reset the zoom/scale
	sprite.Scale = new Vector2(1f, 1f);

	// End animations
	BoostAnimLeft.Stop();
	BoostAnimRight.Stop();


	// Increase velocity in the X direction that snowball is facing
	if (sprite.FlipH) // Facing right
	{
		velocity.X += BoostVelocity;
		Velocity = velocity;
		MoveAndSlide();
	}
	else // Facing left
	{
		velocity.X -= BoostVelocity;;
		Velocity = velocity;
		MoveAndSlide();
	}

	// If you want this boost to be a one-time burst, you can reset the velocity back after some time or distance. Consider using a Timer for that.
}

	void ApplyGravity(float delta) {
		// Add the gravity.
		if (!IsOnFloor()) 
		{
			velocity.Y += gravity * delta;
			
		}
	}

	bool HandleIceTile() {
	// Pressing "down" negates ice physics 
		var retVal = false;
		try {
				var collider = GetLastSlideCollision().GetCollider();
				var colliderRid = GetLastSlideCollision().GetColliderRid();
				if (colliderRid != null) //Todo: Check if this is the correct way to check for null
				{
					//check if standing on staticbody
					if (collider.GetClass() == "StaticBody2D")
						return false;

					var tileData = tileMap.GetCellTileData(0,tileMap.GetCoordsForBodyRid(colliderRid));
					if ((bool)tileData.GetCustomData("Ice") == true && IsOnFloor()) {
						retVal = true;
					}
				}
			} catch (NullReferenceException) {
				// Do nothing
			}
		return retVal;
	}

	private void OnPowerup()
	{
		if (Power < MaxPower)
		{
			EmitSignal(SignalName.Powerup,1);
			Power++;
		}
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
		if (!IsOnFloor()){animation.Stop();}
		

	}
}



