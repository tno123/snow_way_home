using Godot;
using System;

public partial class BlinkingBonfire : Bonfire
{
	private Timer blinkTimer;
	private CollisionShape2D collision;

	[Export]
	bool VisibleStart;

	[Export]
	private bool Hot = true;

	private AnimatedSprite2D animation;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready(); // This will call the _Ready() from the parent class Bonfire
		//Visible = VisibleStart;
		blinkTimer = GetNode<Timer>("BlinkTimer");
		blinkTimer.Start();

		animation = GetNode<AnimatedSprite2D>("BlinkingBonfireAnim");

		collision = GetNode<Area2D>("Area2D").GetNode<CollisionShape2D>("CollisionShape2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta); // This calls the _Process() of the Bonfire, ensuring the animation plays

		if (Hot)
		{
			stopAnimation();
			base.playAnimation();
			//base._Process(delta); // This calls the _Process() of the Bonfire, ensuring the animation plays
		}
		else
		{
			base.stopAnimation();
			playAnimation();
		}
		// Your additional code for BlinkingBonfire goes here
	}

	private void _on_timer_timeout()
	{
		if (Hot)
		{
			Hot = false;
			collision.Disabled = true;
		}
		else
		{
			Hot = true;
			collision.Disabled = false;
		}
	}

	protected override void _on_area_2d_body_entered(Node2D body)
	{
		if (Hot)
		{
			base._on_area_2d_body_entered(body);
		}
	}

	protected override void playAnimation()
	{
		animation.Visible = true;
		animation.Play("default");
	}

	protected override void stopAnimation()
	{
		animation.Visible = false;
		animation.Stop();
	}
}
