using Godot;
using System;

public partial class BossLevel : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var ground = GetNode<StaticBody2D>("Ground");
        var groundChildren = ground.GetChildren();

        for (int i = 0; i < groundChildren.Count; i++)
        {
            if (groundChildren[i] is CollisionPolygon2D)
            {
                var colPolygon = (CollisionPolygon2D)groundChildren[i];
                var polygon = colPolygon.GetNode<Polygon2D>("Polygon2D");
                polygon.Polygon = colPolygon.Polygon;
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
