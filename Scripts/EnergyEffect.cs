using Godot;
using System;

public class EnergyEffect : Node2D
{
    [Export] public bool playSound;
    private GeneralSingleton _generalSingleton;
    private Sprite[] _circles;
    private float _energyValue = 0;

    public override void _Ready()
    {
        _generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
        _circles = new Sprite[3];
        _circles[0] = GetNode<Sprite>("Circle1");
        _circles[1] = GetNode<Sprite>("Circle2");
        _circles[2] = GetNode<Sprite>("Circle3");
    }

    public override void _PhysicsProcess(float delta)
    {
        for (int i = 0; i < _circles.Length; i++)
        {
            _circles[i].Modulate = new Color(1,1,1, (1 - _circles[i].Scale.x) * _energyValue);
            _circles[i].Scale -= Vector2.One * delta * 4f;

            if (_circles[i].Scale.x <= 0)
            {
                _circles[i].Scale += Vector2.One;
                _circles[i].Modulate = new Color(0,0,0,0);
                
                if (playSound)
                {
                    _generalSingleton.PlaySound("wave", (-50 + _circles[i].Scale.x  * _energyValue * 60), 1);
                }
            }
        }
    }

    public float EnergyValue
    {
        set {_energyValue = value;}
    }
}
