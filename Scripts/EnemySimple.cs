using Godot;
using System;

public class EnemySimple : RigidBody2D, IEnemy
{
    private GeneralSingleton _generalSingleton;
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
        _generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
        _animatedSprite = GetNode<AnimatedSprite>("Bat");
    }

    public override void _PhysicsProcess(float delta)
    {
        Rotation = 0;
        if (_player == null && _generalSingleton.PlayerNode != null)
        {
            _player = _generalSingleton.PlayerNode;
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
        if (body is Player && ((Player)body).IsInvinvible == false)
        {
            ((Player)body).TakeDamage();
            _generalSingleton.PlaySound("hit", -10, 1.5f);
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
