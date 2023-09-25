using Godot;
using System;

public partial class Bonfire : Node2D
{
	private AnimatedSprite2D animation;
	protected Snowball Snowball = null;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		animation.Play("default");
	}
	protected virtual void _on_area_2d_body_entered(Node2D body)
	{
		if (body is Snowball){
			Snowball = body as Snowball;
			damageSnowball(Snowball);

		}
	}
	protected virtual void damageSnowball(Snowball snowball){
		snowball.Damage(1);
		snowball.Scale = new Vector2((float)(((Snowball)snowball).Scale.X/1.5), (float)(((Snowball)snowball).Scale.Y/1.5));
	}	
	
}



