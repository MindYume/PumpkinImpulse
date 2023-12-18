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

public class EnemyFast : RigidBody2D, IEnemy
{
    private Player _player;
    private float _health = 125;
    public float Health
    {
        set => _health = value;
        get => _health;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_player == null && GeneralSingleton.Instance.PlayerNode != null)
        {
            _player = GeneralSingleton.Instance.PlayerNode;
        }
        else
        {
            LinearVelocity += (_player.Position-Position) * delta * 1f;
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
            HealthPoint.Spawn(GetParent(), Position, 0.2);
            QueueFree();
        }
    }

    public void _on_EnemyFast_body_entered(Node body)
    {
        if (body is Player)
        {
            ((Player)body).TakeDamage();
            QueueFree();
        }
    }
}
