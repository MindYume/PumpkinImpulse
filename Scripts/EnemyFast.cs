using Godot;

public class EnemyFast : RigidBody2D, IEnemy
{
    private GeneralSingleton _generalSingleton;
    private Player _player;
    private float _health = 125;
    public float Health
    {
        set => _health = value;
        get => _health;
    }
    public override void _Ready()
    {
        _generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_player == null && _generalSingleton.PlayerNode != null)
        {
            _player = _generalSingleton.PlayerNode;
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
