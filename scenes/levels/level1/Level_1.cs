using Godot;
using System;


public partial class Level_1 : Node
{
	[Export]
	public Transitions transitions;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		transitions.SetNextAnimation("fade_in");
		//var StartButtonTexture = GetNode<Area2D>("Start/StartTexture");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void _on_body_entered(Node2D body)
	{
		
		transitions.SetNextAnimation("fade_out");
	}
	
}
