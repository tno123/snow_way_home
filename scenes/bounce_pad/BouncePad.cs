using Godot;
using System;

public partial class BouncePad : Sprite2D
{	
	[Signal] 
	public delegate bool BounceEventHandler();
	
	AnimatedSprite2D animation;
	
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
		
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_area_2d_body_entered(Node2D body)
	{
		/*
		currently using signals, but i think groups are a better solution
		if we are using more than one bouncepad
		*/
		animation.Play("main");
		EmitSignal(SignalName.Bounce);
		
		
	}
}






