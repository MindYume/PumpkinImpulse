using Godot;
using System;

public class Fire : Area2D
{
    private GeneralSingleton _generalSingleton;
    public override void _Ready()
    {
        _generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
        Random rand = new Random();
        _generalSingleton.PlaySound("fire", -15 + (float)rand.NextDouble(), (0.8f + ((float)rand.NextDouble())/2.5f));
    }

    public override void _PhysicsProcess(float delta)
    {
        Scale -= new Vector2(1,1) * delta * 0.1f;
        if (Scale.x <= 0.65f)
        {
            QueueFree();
        }
    }

    public void _on_Fire_body_entered(Node body)
    {
        if (body is Player)
        {
            ((Player)body).TakeDamage();
            QueueFree();
        }
    }
}
