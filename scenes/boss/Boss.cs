using Godot;
using System;

public partial class Boss : Node2D
{
	public float damping = 0.1f;
	public float maxVelocity = 300.0f;
	private Snowball snowball;
	private BossHand leftHand;
	private BossHand rightHand;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		snowball = GetParent().GetNode<Snowball>("Snowball");
		leftHand = GetNode<BossHand>("BossHand2");
		rightHand = GetNode<BossHand>("BossHand");

		//Set the initial position of the hands
		leftHand.GlobalPosition = new Vector2(snowball.Position.X - 100, snowball.Position.Y - 100);
		rightHand.GlobalPosition = new Vector2(
			snowball.Position.X + 100,
			snowball.Position.Y - 100
		);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateHands((float)delta);
	}

	// Update the position of the hands to follow the snowball and move in a small circle
	private void UpdateHands(float delta)
	{
		// Calculate the target position of the hands based on the snowball's position
		Vector2 targetLeft = new Vector2(snowball.Position.X - 100, snowball.Position.Y - 100);
		Vector2 targetRight = new Vector2(snowball.Position.X + 100, snowball.Position.Y - 100);

		// Apply drag to the hands' velocities
		leftHand.Velocity = leftHand.Velocity.Lerp(
			(targetLeft - leftHand.GlobalPosition) / (delta * 10),
			damping
		);
		rightHand.Velocity = rightHand.Velocity.Lerp(
			(targetRight - rightHand.GlobalPosition) / (delta * 10),
			damping
		);

		// Clamp the velocity of the hands to the maximum value
		if (leftHand.Velocity.Length() > maxVelocity)
		{
			leftHand.Velocity = leftHand.Velocity.Normalized() * maxVelocity;
		}
		if (rightHand.Velocity.Length() > maxVelocity)
		{
			rightHand.Velocity = rightHand.Velocity.Normalized() * maxVelocity;
		}

		// Update the position of the hands based on their velocities
		leftHand.GlobalPosition += leftHand.Velocity * delta;
		rightHand.GlobalPosition += rightHand.Velocity * delta;

		// Make the hands move in a small circle
		leftHand.Position += new Vector2(
			Mathf.Cos(Time.GetTicksMsec() / 500.0f) * 5,
			Mathf.Sin(Time.GetTicksMsec() / 500.0f) * 5
		);
		rightHand.Position += new Vector2(
			Mathf.Cos(Time.GetTicksMsec() / 500.0f) * 5,
			Mathf.Sin(Time.GetTicksMsec() / 500.0f) * 5
		);
	}
}
