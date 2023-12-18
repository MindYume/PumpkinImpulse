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

public class EnemySimple : RigidBody2D, IEnemy
{
    private Player _player;
    private AnimatedSprite _animatedSprite;
    private float _health = 100;
    private float _maxSpeed = 125;
    private float _acceleration = 100;
    public float Health
    {
        set => _health = value;
        get => _health;
    }

    public override void _Ready()
    {
        _animatedSprite = GetNode<AnimatedSprite>("Bat");
    }

    public override void _PhysicsProcess(float delta)
    {
        Rotation = 0;
        if (_player == null && GeneralSingleton.Instance.PlayerNode != null)
        {
            _player = GeneralSingleton.Instance.PlayerNode;
        }
        else
        {
            Vector2 relativePlayerPosition = _player.Position - Position;
            float LinearVelocityAtan2 = Mathf.Atan2(LinearVelocity.y, LinearVelocity.x);
            float relativePlayerPositionAtan2 = Mathf.Atan2(relativePlayerPosition.y, relativePlayerPosition.x);
            float directionDifference = LinearVelocityAtan2 - relativePlayerPositionAtan2;
            if (Mathf.Cos(directionDifference) * LinearVelocity.Length() < _maxSpeed)
            {
                LinearVelocity += (_player.Position-Position).Normalized()*_acceleration * delta;
            }

            if (_player.Position.x > Position.x)
            {
                _animatedSprite.FlipH = true;
            }
            else
            {
                _animatedSprite.FlipH = false;
            }
        }

        if (Position.x < 0 || Position.x > 600 || Position.y < 0 || Position.y > 600)
        {
            GD.Print("Out of border");
            QueueFree();
        }
    }

    public void TakeDamage(float damage_value)
    {
        _health -= damage_value;
        if (_health <= 0)
        {
            HealthPoint.Spawn(GetParent(), Position, 0.1);
            QueueFree();
        }
    }

    public void _on_EnemySimple_body_entered(Node body)
    {
        if (body is Player && ((Player)body).IsInvincible == false)
        {
            ((Player)body).TakeDamage();
            SoundPlayer.PlaySound("hit", -10, 1.5f);
            HitEffect.Create(
				(Node2D)GetParent().GetParent(),
				(GlobalPosition + ((Node2D)body).GlobalPosition)/2,
				Vector2.One * 1.75f,
				new Color("bfffffff")
			);
            QueueFree();
        }
    }
}
