using Godot;
using System;
using System.Collections;

public partial class SpawnEnemy : Area2D
{
	//private PackedScene VillagerMobScene = GD.Load<PackedScene>("res://scenes/VillagerMob.tscn");
	private bool enemySpawned = false;

	[Export]
	private bool spawnFromLeft = false;
	private float distanceFromSnowball = 700;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() { }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) { }

	private void _on_body_entered(Node2D body)
	{
		if (body is Snowball && !enemySpawned)
		{
			var enemiesNode = GetParent().GetNode<Node>("Enemies");
			foreach (VillagerMob mob in enemiesNode.GetChildren())
			{
				mob.QueueFree();
			}

			var VillagerMobScene = GD.Load<PackedScene>("res://scenes/VillagerMob.tscn");
			var instance = (VillagerMob)VillagerMobScene.Instantiate();
			var snowBallPosition = body.Position;

			instance.Position = new Vector2(
				snowBallPosition.X + (spawnFromLeft ? -distanceFromSnowball : distanceFromSnowball),
				snowBallPosition.Y
			);
			GetParent().GetNode<Node>("Enemies").AddChild(instance);







			enemySpawned = true;
		}
	}
}
