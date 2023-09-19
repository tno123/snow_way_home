using Godot;
using System;

public partial class BlinkingBonfire : Bonfire
{
	private Timer blinkTimer;
	
	[Export] 
	bool VisibleStart;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		base._Ready(); // This will call the _Ready() from the parent class Bonfire
		Visible = VisibleStart;

		blinkTimer = GetNode<Timer>("BlinkTimer");
	 	blinkTimer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta); // This calls the _Process() of the Bonfire, ensuring the animation plays

		// Your additional code for BlinkingBonfire goes here
	}
	private void _on_timer_timeout()
	{
		if (Visible == true){
			Visible = false;
		}
		else{Visible = true; }
		
	}
	
	protected override void _on_area_2d_body_entered(Node2D body)
	{
		if (Visible)
		{
			base._on_area_2d_body_entered(body);
		}
	}
}






