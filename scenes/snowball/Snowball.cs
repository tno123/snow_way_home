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

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
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

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction.X != 0)
		{
			if (!HandleIceTile())
				
			velocity.X = direction.X * Speed;
			
			animation.Play("moving_left");
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
		//if (jumped) {
		//}
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
}
