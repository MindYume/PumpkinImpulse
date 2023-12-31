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

public class Player : RigidBody2D
{
    private PackedScene _bulletPreload = GD.Load<PackedScene>("res://Objects/Bullet.tscn");
    private AnimationPlayer _animationPlayer;
    private AnimatedSprite _animatedSprite;
    private Timer invincibilityTimer;
    private Line2D _arrorw;
    private EnergyEffect _energyEffect;
    private float _rotationDirection = -7.5f;
    private int _health = 3;
    private float _bulletPower = 0;
    private bool _isPlayerButtonPressed = false;
    private bool _isPlayerButtonJustReleased = false;
    private bool _isInvinvible = false;
    private float invincibilityDuration = 2;

    public bool IsInvincible => _isInvinvible;

    [Signal] public delegate void health_value_changed(int newHealthValue);
    [Signal] public delegate void dead();

    public override void _Ready()
    {
        GeneralSingleton.Instance.PlayerNode = this;
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        invincibilityTimer = GetNode<Timer>("invincibilityTimer");
        invincibilityTimer.WaitTime = invincibilityDuration;
        _arrorw = GetNode<Line2D>("Arrow");
        _energyEffect = GetNode<EnergyEffect>("EnergyEffect");
    }

    public override void _PhysicsProcess(float delta)
    {
        Rotation = 0;
        _arrorw.Rotation += _rotationDirection * delta * (0.1f+_bulletPower*0.9f);
        if (Mathf.Abs(_arrorw.GlobalRotation) > Mathf.Pi) { _arrorw.GlobalRotation = Mathf.Clamp(_arrorw.GlobalRotation, -1, 1) * (Mathf.Abs(_arrorw.GlobalRotation) - Mathf.Pi*2); }

        if (Input.IsActionPressed("mouse_left") || _isPlayerButtonPressed)
        {
            _bulletPower += delta * 2f;
            _bulletPower = Mathf.Clamp(_bulletPower, 0, 1);
            _energyEffect.EnergyValue = _bulletPower;
        }

        if (Input.IsActionJustReleased("mouse_left") || _isPlayerButtonJustReleased)
        {
            _rotationDirection = -_rotationDirection;
            SpawnBullet(GlobalPosition + (new Vector2(Mathf.Sin(_arrorw.GlobalRotation), -Mathf.Cos(_arrorw.GlobalRotation))) * 30, _arrorw.GlobalRotation, _bulletPower);
            LinearVelocity -= (new Vector2(Mathf.Sin(_arrorw.GlobalRotation), -Mathf.Cos(_arrorw.GlobalRotation))) * _bulletPower * 300;
            _bulletPower = 0;
            _energyEffect.EnergyValue = 0;
        }

        _isPlayerButtonJustReleased = false;

        if (LinearVelocity.x > 0)
        {
            _animatedSprite.FlipH = true;
            _animatedSprite.Position = new Vector2(-4, 1);
        }
        else
        {
            _animatedSprite.FlipH = false;
            _animatedSprite.Position = new Vector2(4, 1);
        }

        if (Position.x < 0 || Position.x > 600 || Position.y < 0 || Position.y > 600)
        {
            Position = new Vector2(300, 300);
            GD.Print("Player is out of border");
        }
    }

    private void SpawnBullet(Vector2 position, float direction, float power)
    {
        Bullet bullet = _bulletPreload.Instance<Bullet>();
        GeneralSingleton.Instance.BulletsContainer.AddChild(bullet);
        bullet.GlobalPosition = position;
        bullet.LinearVelocity += (new Vector2(Mathf.Sin(direction), -Mathf.Cos(direction))) * power * 450;
        bullet.SetPower(_bulletPower);

        Random rand = new Random();
        SoundPlayer.PlaySound("wave_end", (-10 + _bulletPower * 15), (0.75f / _bulletPower));
    }

    public void TakeDamage()
    {
        if (!_isInvinvible)
        {
            _health -= 1;
            EmitSignal(nameof(health_value_changed), _health);

            if (_health <= 0)
            {
                EmitSignal(nameof(dead));
                SoundPlayer.PlaySound("game_over", -10, 1);
            }

            _isInvinvible = true;
            _animationPlayer.Play("PlayBlink");
            invincibilityTimer.Start();

            Random rand = new Random();
            switch(rand.Next(0,3))
            {
                case 0:
                    SoundPlayer.PlaySound("take_damage1", -5, 1.1f);
                    break;
                case 1:
                    SoundPlayer.PlaySound("take_damage2", -5, 1.1f);
                    break;
                case 2:
                    SoundPlayer.PlaySound("take_damage3", -10, 1.1f);
                    break;
            }
        }
    }

    public void TakeHealth()
    {
        _health++;
        EmitSignal(nameof(health_value_changed), _health);
    }

    public void _on_invincibilityTimer_timeout()
    {
        _isInvinvible = false;
        _animationPlayer.Play("StopBlink");
    }

    public void _on_PlayerButton_button_down()
    {
        _isPlayerButtonPressed = true;
    }

    public void _on_PlayerButton_button_up()
    {
        _isPlayerButtonPressed = false;
        _isPlayerButtonJustReleased = true;
    }
}
