using Godot;
using System;

public partial class MountainLevel : Node
{
	AnimatedSprite2D snowDiveInfo;
	Area2D infoBoard;
	bool infoAvailable = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		infoBoard = GetNode<Area2D>("infoBoard");
		snowDiveInfo = GetNode<CanvasLayer>("UI").GetNode<AnimatedSprite2D>("snowDiveInfo");
		snowDiveInfo.Hide();
		//Get all CollisionPolygon2D that are child nodes of Ground
		//snowDiveInfo = GetNode<AnimatedSprite2D>("snowDiveInfo");
		var ground = GetNode<Node2D>("Ground");
		var groundChildren = ground.GetChildren();

		for (int i = 0; i < groundChildren.Count; i++)
		{
			if (groundChildren[i] is CollisionPolygon2D)
			{
				var colPolygon = (CollisionPolygon2D)groundChildren[i];
				var polygon = colPolygon.GetNode<Polygon2D>("Polygon2D");
				polygon.Polygon = colPolygon.Polygon;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("showInfo") && infoAvailable)
		{
			snowDiveInfo.Show();
			snowDiveInfo.Play("default");
		}
		else if (Input.IsActionPressed("hideInfo"))
		{
			snowDiveInfo.Hide();
			snowDiveInfo.Stop();
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
			snowDiveInfo.Hide();
			snowDiveInfo.Stop();
		}
	}
}
