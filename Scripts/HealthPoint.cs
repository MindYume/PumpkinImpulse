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

public class HealthPoint : Area2D
{
    private Player _player;

    public override void _Ready()
    {
        _player = GeneralSingleton.Instance.PlayerNode;
    }

    public override void _PhysicsProcess(float delta)
    {
        float distanceToPlayer = (_player.Position-Position).Length();
        if (distanceToPlayer < 75)
        {
            Position += (_player.Position-Position) * 10 * delta;
        }
    }

    public void _on_Health_body_entered(Node body)
    {
        if (body is Player)
        {
            ((Player)body).TakeHealth();
            SoundPlayer.PlaySound("healthpoint", 0, 1);
            QueueFree();
        }
    }

    public static void Spawn(Node parent, Vector2 position, double spawnProbability)
    {
        Random rand = new Random();
        if (rand.NextDouble() <= spawnProbability)
        {
            PackedScene _healthPreload = GD.Load<PackedScene>("res://Objects/HealthPoint.tscn");
            HealthPoint health = _healthPreload.Instance<HealthPoint>();
            parent.CallDeferred("add_child", health);
            health.Position = position;
        }
    }
}
