using Godot;
using System;

public partial class BossHand : Node2D
{
    [Export]
    public bool flipped = false;

    [Signal]
    public delegate void BossDamageEventHandler();
    public string State = "idle";
    public Vector2 Velocity = Vector2.Zero;
    public bool hit = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (flipped)
        {
            GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Position += Velocity * (float)delta;
    }

    private void _on_area_2d_body_entered(Node2D body)
    {
        if (body is Snowball)
        {
            //If snowball enters from above, damage boss
            if (body.Position.Y < GlobalPosition.Y)
            {
                EmitSignal(SignalName.BossDamage);
            }

            //Else damage snowball
            hit = true;
            ((Snowball)body).Damage(1);
        }
        else if (body is StaticBody2D)
        {
            hit = true;
        }
    }
}
