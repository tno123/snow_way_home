using Godot;
using System;
using System.Threading.Tasks;


public partial class Cutscene : Node2D
{
	
	public Camera2D camera;
	public Vector2 previousPosition; 
	public RemoteTransform2D remoteTransform;
	public Snowball snowball;
	private bool cameraReturningToPlayer;
	
 	[Export]
	public Vector2 offset = new Vector2(250, 0); // Default offset values, can be changed in the Godot Editor.

	[Export]
	public float stopTimeDuration = 2.0f; // Default stop time duration.
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		camera =GetParent().GetNode<Camera2D>("Camera2D");
		snowball = GetParent().GetNode<Snowball>("Snowball");
		remoteTransform = GetParent().GetNode<Snowball>("Snowball").GetNode<RemoteTransform2D>("RemoteTransform2D");
		cameraReturningToPlayer = false;  
		
		GetNode<Timer>("StopTime").WaitTime = stopTimeDuration;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		/*
		if (cameraReturningToPlayer)
		{
			//float distance = camera.GlobalPosition.DistanceTo(remoteTransform.GlobalPosition);
			GD.Print(distance);
			
			if (distance < 5.0f)  // Here 5.0f is a threshold. You can adjust it based on your needs.
				{
					GD.Print("test");
					camera.PositionSmoothingEnabled = false;
					cameraReturningToPlayer = false;
					QueueFree();
					snowball.StartMovement();
				}
		}
		*/
		

		
	}
	private async void _on_area_2d_body_entered(Node2D body)
	{
		
		camera.PositionSmoothingEnabled=true;
		previousPosition = new Vector2(remoteTransform.Position.X, remoteTransform.Position.Y);
		remoteTransform.Position += offset; // Use the offset property here
		snowball.StopMovement();

		/*
		Må kanskje bruke to timers (panIn og panOut)
		panIn spiller, når den er ferdig spilles panOut, når panOut er ferdig --> kjør queue_free() og camera.PositionSmoothingEnabled=false;

		Alt dette gjøres i et forsøk på å ha smooth transition når man skal se på bonfire, men unngå smooth transitioning ellers (det ser rart ut)
		*/

		StopTime();
	}
	
	private void StopTime()
	{
		GetNode<Timer>("StopTime").Start();
		
	}
	private void _on_timer_timeout()
	{
		//cameraReturningToPlayer = true;  // Set the flag to true when the timer times out.
		remoteTransform.Position =previousPosition;
		
		QueueFree();
		snowball.StartMovement();
		camera.PositionSmoothingEnabled=false;
					
		//if (remoteTransform.Position ==previousPosition){
		//	camera.PositionSmoothingEnabled=false;
		//}
	 	
	}
	
	}






