using Godot;
using System;

public partial class Boss : Node2D
{
	public float damping = 0.1f;
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
		//Update the position of the hands to follow Snowball, and make the hands move in a small circle
		Vector2 targetLeft = new Vector2(snowball.Position.X - 100, snowball.Position.Y - 100);
		Vector2 targetRight = new Vector2(snowball.Position.X + 100, snowball.Position.Y - 100);

		//Apply drag to the hands
		leftHand.Velocity = leftHand.Velocity.Lerp(
			(targetLeft - leftHand.GlobalPosition) / ((float)delta * 10),
			damping
		);
		rightHand.Velocity = rightHand.Velocity.Lerp(
			(targetRight - rightHand.GlobalPosition) / ((float)delta * 10),
			damping
		);

		leftHand.GlobalPosition += leftHand.Velocity * (float)delta;
		rightHand.GlobalPosition += rightHand.Velocity * (float)delta;

		//Make the hands move in a small circle
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
