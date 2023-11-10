using Godot;
using System;

public class Healthbar : TextureRect
{
    private int _heartsAmont;
    public int HeartsAmont
    {
        set
        {
            _heartsAmont = value;
            SetSize(new Vector2(_heartsAmont * 32, 28));
        }

        get {return _heartsAmont;}
    }

    public override void _Ready()
    {
        HeartsAmont = 3;
    }

    public void _on_Player_health_value_changed(int newHealthValue)
    {
        HeartsAmont = newHealthValue;
    }
}
