using Godot;
using System;

public partial class Exclamation : AnimatedSprite2D
{
	private void _on_animation_finished()
{
	QueueFree();
}
}



