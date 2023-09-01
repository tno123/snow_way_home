using Godot;
using System;

public partial class Cutscene : Node2D
{
	
	public Camera2D camera;
	public Vector2 previousPosition; 
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		camera = GetNode<Snowball>("Snowball").GetNode<Camera2D>("Camera2D");
		//previousPosition = new Vector2(camera.Position.X, camera.Position.Y);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print(previousPosition);
	}
	private void _on_area_2d_body_entered(Node2D body)
	{
		if (body is Snowball){
			//GD.Print(previousPosition);
			
		}
	}
}



