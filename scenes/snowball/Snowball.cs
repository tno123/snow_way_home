using Godot;
using System;

public partial class Snowball : CharacterBody2D
{
	[Export]
	public float IceSpeed = 10.0f;

	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	public const float CoyoteTime = 0.1f;

	public TileMap tileMap;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		tileMap = GetParent().GetNode<TileMap>("TileMap");
		bool isOnIce = false;

		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

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
			// Do try except on get last slide collision
			try {
				var colliderRid = GetLastSlideCollision().GetColliderRid();
				if (colliderRid != null)
				{
					var tileData = tileMap.GetCellTileData(0,tileMap.GetCoordsForBodyRid(colliderRid));
					if ((bool)tileData.GetCustomData("Ice") == true && IsOnFloor()) {
						isOnIce = true;
					}
				}
			} catch (NullReferenceException) {
				// Do nothing
			}
			
			//Friction
			velocity.X = Mathf.MoveToward(Velocity.X, 0, isOnIce? IceSpeed : Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
