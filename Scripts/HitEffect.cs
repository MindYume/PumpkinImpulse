using Godot;
using System;

public class HitEffect : Sprite
{
    private float _time = 0;
    private float _animationNum = 0;
    private Random _rand;
    public override void _Ready()
    {
        _rand = new Random();
        _animationNum = _rand.Next(3);
        FrameCoords = new Vector2(_animationNum, 0);
    }

    public override void _Process(float delta)
    {
       _time += delta;

        if (_time > 0.15f)
        {
            QueueFree();
        }

        if (_time > 0.075f)
        {
            FrameCoords = new Vector2(_animationNum, 1);
        }
    }

    public static void Create(Node2D parentNode, Vector2 globalPosition, Vector2 scale, Color color)
    {
        PackedScene _hitEffectPreload = GD.Load<PackedScene>("res://Objects/HitEffect.tscn");
        HitEffect hitEffect = _hitEffectPreload.Instance<HitEffect>();
        parentNode.AddChild(hitEffect);
        hitEffect.GlobalPosition = globalPosition;
        hitEffect.Scale = scale;
        hitEffect.Modulate = color;
    }
}
