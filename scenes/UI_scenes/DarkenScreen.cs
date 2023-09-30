using Godot;
using System;

public partial class DarkenScreen : Control
{
    public ColorRect DarkenRect;
    private bool Darken = false;
    private Tween tween;
    private Tween resetTween;
    private Snowball snowball;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        snowball = GetParent().GetParent().GetNode<Snowball>("Snowball");
        snowball.DarkenScreen += OnDarkenScreen;
        DarkenRect = GetNode<ColorRect>("ColorRect");
        tween = CreateTween();
        tween.TweenProperty(
            DarkenRect,
            "color",
            new Color(0.3f, 0.3f, 0.3f, 1.0f),
            snowball.DarkenTime
        );
        tween.Stop();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Darken && !tween.IsRunning())
        {
            if (!tween.IsValid() || tween == null)
            {
                tween = CreateTween();
                tween.TweenProperty(
                    DarkenRect,
                    "color",
                    new Color(0.3f, 0.3f, 0.3f, 1.0f),
                    snowball.DarkenTime
                );
            }
            tween.Play();
        }
        else if (!Darken && tween.IsRunning())
        {
            tween.Stop();
            DarkenRect.Color = new Color(0.3f, 0.3f, 0.3f, 0.0f);
        }
    }

    void OnDarkenScreen(bool darken)
    {
        Darken = darken;
    }
}
