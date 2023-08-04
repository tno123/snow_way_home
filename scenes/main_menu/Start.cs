using Godot;
using System;

public partial class Start : Button
{
	[Export]
	public Transitions transitions;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	/*
	private void _on_toggled(bool button_pressed)
	{
		transitions.SetNextAnimation(button_pressed);
	}
	*/
}



