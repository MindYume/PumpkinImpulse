using Godot;

public class EnemyOrbit : RigidBody2D, IEnemy
{
    private GeneralSingleton _generalSingleton;
    private Player _player;
    private AnimatedSprite _animatedSprite;
    private float _health = 200;
    public float Health
    {
        set => _health = value;
        get => _health;
    }
    public override void _Ready()
    {
        _generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
        _animatedSprite = GetNode<AnimatedSprite>("Ghost");
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
            float distanceToPlayer = (_player.Position-Position).Length();
            LinearVelocity += (_player.Position-Position).Normalized()/Mathf.Pow(distanceToPlayer, 2) * 4500000 * delta;
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
            HealthPoint.Spawn(GetParent(), Position, 0.3);
            QueueFree();
        }
    }

    public void _on_EnemyOrbit_body_entered(Node body)
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
