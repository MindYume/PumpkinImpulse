/*
MIT License

Copyright (c) 2023 Viktor Grachev

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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

        if (_time > 0.075f)
        {
            FrameCoords = new Vector2(_animationNum, 1);
        }

        if (_time > 0.15f)
        {
            QueueFree();
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
