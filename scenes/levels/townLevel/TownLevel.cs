using Godot;
using System;

public partial class TownLevel : Node
{
	// Called when the node enters the scene tree for the first time.
	AnimatedSprite2D boostInfo;
	ProgressBar SnowPower;
	Area2D infoBoard;

	bool playing = false;
	bool infoAvailable = false;

	public override void _Ready()
	{
		infoBoard = GetNode<Area2D>("infoBoard");
		boostInfo = GetNode<CanvasLayer>("UI").GetNode<AnimatedSprite2D>("boostInfo");
		boostInfo.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("showInfo") && infoAvailable)
		{
			boostInfo.Show();
			boostInfo.Play("default");
		}
		else if (Input.IsActionPressed("hideInfo"))
		{
			boostInfo.Hide();
			boostInfo.Stop();
		}
	}

	private void _on_info_board_body_entered(Node2D body)
	{
		if (body is Snowball)
		{
			infoAvailable = true;
			infoBoard.GetNode<AnimatedSprite2D>("boardArrow").Visible = true;
			infoBoard.GetNode<AnimatedSprite2D>("boardArrow").Play("default");
		}
	}

	private void _on_info_board_body_exited(Node2D body)
	{
		if (body is Snowball)
		{
			infoAvailable = false;
			infoBoard.GetNode<AnimatedSprite2D>("boardArrow").Visible = false;
			boostInfo.Hide();
			boostInfo.Stop();
		}
	}
}
