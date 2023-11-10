using Godot;
using System;

public class EnemyGun : RigidBody2D, IEnemy
{
    private PackedScene _bulletPreload = GD.Load<PackedScene>("res://Objects/Bullet.tscn");
    private GeneralSingleton _generalSingleton;
    private Player _player;
    private AnimationTree _animationTree;
    private float _health = 200;
    private float _reloadDelay = 1.5f;
    private float _reloadTime = 0;
    private float _lookDirection = 0;
    Vector2 _moveTargetPosition = Vector2.Zero;
    public float Health
    {
        set => _health = value;
        get => _health;
    }

    public override void _Ready()
    {
        _generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
        _animationTree = GetNode<AnimationTree>("AnimationTree");
    }

    public override void _PhysicsProcess(float delta)
    {
        Rotation = 0;
        if (_player != null)
        {
            Vector2 relativePlayerPosition = _player.Position - Position;
            _lookDirection = Mathf.Atan2(relativePlayerPosition.y, relativePlayerPosition.x) + Mathf.Pi / 2;
            float distanceToPlayer = (_player.Position-Position).Length();
            if (distanceToPlayer > 200)
            {
                LinearVelocity += (_player.Position-Position) * delta * 1f;
            }
            else
            {
                LinearVelocity -= (_player.Position-Position) * delta * 1f;
            }


            _reloadTime += delta;
            if (_reloadTime >= _reloadDelay)
            {
                _reloadTime -= _reloadDelay;

                // Spawn bullet
                Bullet bullet = _bulletPreload.Instance<Bullet>();
                //GetParent().AddChild(bullet);
                _generalSingleton.BulletsContainer.AddChild(bullet);
                bullet.GlobalPosition = Position;
                bullet.LinearVelocity += (new Vector2(Mathf.Sin(_lookDirection), -Mathf.Cos(_lookDirection))) * 250;
                bullet.SetPower(0.7f);
                bullet.SetTarget("player");
                bullet.SetColor(Color.Color8(255, 200, 100), Color.Color8(255, 125, 0), Color.Color8(255, 255, 200));

                Random rand = new Random();
                _generalSingleton.PlaySound("fire", -15 + (float)rand.NextDouble(), (0.8f + ((float)rand.NextDouble())/2.5f));
                _generalSingleton.PlaySound("wave_end", -5 + (float)rand.NextDouble(), (0.8f + ((float)rand.NextDouble())/2.5f));
            }
            _animationTree.Set("parameters/EyesDirection/blend_position", _player.Position - Position);

            if (Position.x < 0 || Position.x > 600 || Position.y < 0 || Position.y > 600)
            {
                GD.Print("Out of border");
                QueueFree();
            }
        }
        else if (_generalSingleton.PlayerNode != null)
        {
            _player = _generalSingleton.PlayerNode;
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
