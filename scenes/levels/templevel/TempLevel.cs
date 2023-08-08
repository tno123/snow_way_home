using Godot;
using System;


public partial class TempLevel : Node
{
	[Export]
	public Transitions transition;

	[Export]
	public Polygon2D polygon;

	[Export]
	public CollisionPolygon2D collisionPolygon;
	
	Camera2D camera;
	CharacterBody2D snowball;
	private string currentScenePath;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		transition.Visible = true;
		transition.SetNextAnimation("fade_in");
		camera = GetNode<Camera2D>("Camera2D");
		snowball = GetNode<CharacterBody2D>("Snowball");
		currentScenePath = "res://scenes/levels/templevel/TempLevel.tscn";
		// 
		
	
		polygon.Polygon = collisionPolygon.Polygon;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
			//GetTree().reload_current_scene();
			if (snowball.Position.Y > camera.LimitBottom){
				GD.Print(snowball.Position.Y);
				GD.Print(camera.LimitBottom);
				var sceneManager = GetNode<SceneManager>("/root/SceneManager");
				//GetTree().ChangeScene(currentScenePath);
				sceneManager.GotoScene(currentScenePath);


			}
			
			
			
			
	}
}



