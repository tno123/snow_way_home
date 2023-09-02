using Godot;
using System;
using System.Threading.Tasks;


public partial class Cutscene : Node2D
{
	
	public Camera2D camera;
	public Vector2 previousPosition; 
	public RemoteTransform2D remoteTransform;
	public Snowball snowball;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		camera =GetParent().GetNode<Camera2D>("Camera2D");
		snowball = GetParent().GetNode<Snowball>("Snowball");
		remoteTransform = GetParent().GetNode<Snowball>("Snowball").GetNode<RemoteTransform2D>("RemoteTransform2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		
	}
	private async void _on_area_2d_body_entered(Node2D body)
	{
		
		camera.PositionSmoothingEnabled=true;
		previousPosition = new Vector2(remoteTransform.Position.X, remoteTransform.Position.Y);
		remoteTransform.Position =new Vector2(remoteTransform.Position.X+250, remoteTransform.Position.Y+500);
		
		snowball.StopMovement();
		camera.RotationSmoothingEnabled=true;
		StopTime();
	}
	
	private void StopTime()
	{
		GetNode<Timer>("StopTime").Start();
		
	}
	private void _on_timer_timeout()
	{
		remoteTransform.Position =previousPosition;
		camera.PositionSmoothingEnabled=false;
	 	QueueFree();
		snowball.StartMovement();
	}
	}






