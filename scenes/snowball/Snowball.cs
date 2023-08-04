using Godot;
using System;

public partial class Snowball : CharacterBody2D
{
	[Export]
	public float IceSpeed = 10.0f;

	[Export]
	public float CoyoteTime = 0.1f;

	[Export]
	public float NextJumpTime = 0.1f;

	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public TileMap tileMap;

	private Timer CoyoteJumpTimer;
	private Timer NextJumpTimer;
	private Vector2 velocity = Vector2.Zero;
	private bool WasOnFloor = false;
	private bool IsOnIce = false;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		CoyoteJumpTimer = GetNode<Timer>("CoyoteJumpTimer");
		NextJumpTimer = GetNode<Timer>("NextJumpTimer");
		CoyoteJumpTimer.WaitTime = CoyoteTime;
		NextJumpTimer.WaitTime = NextJumpTime;
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
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			IsOnIce = HandleIceTile();
			
			//Friction
			velocity.X = Mathf.MoveToward(Velocity.X, 0, IsOnIce? IceSpeed : Speed);
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
		}
	}

	void HandleJump() {
		// Handle Jump, Coyote Jump, and Next Jump
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor() ||
			!IsOnFloor() && Input.IsActionPressed("ui_accept") && CoyoteJumpTimer.TimeLeft > 0)
		{
			//NextJumpTimer.Start();
			velocity.Y = JumpVelocity;
			CoyoteJumpTimer.Stop();
			NextJumpTimer.Start();
		}
		
		if (Input.IsActionJustPressed("ui_accept") && !IsOnFloor() && NextJumpTimer.IsStopped())
		{
			NextJumpTimer.Start();
			velocity.Y = JumpVelocity;
		}
	}

	void ApplyGravity(float delta) {
		// Add the gravity.
		if (!IsOnFloor()) 
		{
			velocity.Y += gravity * delta;
		}
	}

	bool HandleIceTile() {
		var retVal = false;
		try {
				var colliderRid = GetLastSlideCollision().GetColliderRid();
				if (colliderRid != null) //Todo: Check if this is the correct way to check for null
				{
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
}
