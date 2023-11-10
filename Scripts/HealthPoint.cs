using Godot;
using System;

public class HealthPoint : Area2D
{
    private GeneralSingleton _generalSingleton;
    private Player _player;

    public override void _Ready()
    {
        _generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
        _player = _generalSingleton.PlayerNode;
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
            _generalSingleton.PlaySound("healthpoint", 0, 1);
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
