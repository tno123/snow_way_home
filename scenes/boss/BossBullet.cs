using Godot;
using System;

public partial class BossBullet : Node2D
{
    public bool Right = true;
    public float Speed = 200.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("default");
        if (Right)
        {
            GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
        }
        else
        {
            GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Right)
        {
            Position += new Vector2((float)delta * Speed, 0);
        }
        else
        {
            Position -= new Vector2((float)delta * Speed, 0);
        }
        if (Position.X < -10000 || Position.X > 10000)
            QueueFree();
    }

    private void _on_area_2d_body_entered(Node2D body)
    {
        if (body is Snowball)
        {
            ((Snowball)body).Damage(1);
            QueueFree();
        }
    }
}
