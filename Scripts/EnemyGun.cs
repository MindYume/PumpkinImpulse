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

public class EnemyGun : RigidBody2D, IEnemy
{
    private PackedScene _bulletPreload = GD.Load<PackedScene>("res://Objects/Bullet.tscn");
    private Player _player;
    private AnimationTree _animationTree;
    private float _health = 200;
    private float _reloadDelay = 1.5f;
    private float _reloadTime = 0;
    public float Health
    {
        set => _health = value;
        get => _health;
    }

    public override void _Ready()
    {
        _animationTree = GetNode<AnimationTree>("AnimationTree");
    }

    public override void _PhysicsProcess(float delta)
    {
        Rotation = 0;
        if (_player != null)
        {
            Vector2 relativePlayerPosition = _player.Position - Position;
            float distanceToPlayer = relativePlayerPosition.Length();
            if (distanceToPlayer > 200)
            {
                LinearVelocity += relativePlayerPosition * delta * 1f;
            }
            else
            {
                LinearVelocity -= relativePlayerPosition * delta * 1f;
            }


            _reloadTime += delta;
            if (_reloadTime >= _reloadDelay)
            {
                _reloadTime -= _reloadDelay;

                // Spawn bullet
                Bullet bullet = _bulletPreload.Instance<Bullet>();
                //GetParent().AddChild(bullet);
                GeneralSingleton.Instance.BulletsContainer.AddChild(bullet);
                bullet.GlobalPosition = Position;
                bullet.LinearVelocity += relativePlayerPosition.Normalized() * 250;
                bullet.SetPower(0.7f);
                bullet.SetTarget("player");
                bullet.SetColor(Color.Color8(255, 200, 100), Color.Color8(255, 125, 0), Color.Color8(255, 255, 200));

                Random rand = new Random();
                SoundPlayer.PlaySound("fire", -15 + (float)rand.NextDouble(), (0.8f + ((float)rand.NextDouble())/2.5f));
                SoundPlayer.PlaySound("wave_end", -5 + (float)rand.NextDouble(), (0.8f + ((float)rand.NextDouble())/2.5f));
            }
            _animationTree.Set("parameters/EyesDirection/blend_position", relativePlayerPosition);

            if (Position.x < 0 || Position.x > 600 || Position.y < 0 || Position.y > 600)
            {
                GD.Print("Out of border");
                QueueFree();
            }
        }
        else if (GeneralSingleton.Instance.PlayerNode != null)
        {
            _player = GeneralSingleton.Instance.PlayerNode;
        }
    }

    public void TakeDamage(float damage_value)
    {
        _health -= damage_value;
        if (_health <= 0)
        {
            HealthPoint.Spawn(GetParent(), Position, 0.5);
            QueueFree();
        }
    }
}
