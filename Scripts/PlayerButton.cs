using Godot;
using System;

public class PlayerButton : TextureButton
{
    private EnergyEffect _energyEffect;
    private float _timeAfterPress = 0;
    private bool _isPressed = false;
    public override void _Ready()
    {
        _energyEffect = GetNode<EnergyEffect>("EnergyEffect");
    }


    public override void _Process(float delta)
    {
        if (_isPressed)
        {
            _timeAfterPress += delta * 2;
            _energyEffect.EnergyValue = Mathf.Clamp(_timeAfterPress, 0, 1);
        }
        else
        {
            _timeAfterPress = 0;
            _energyEffect.EnergyValue = 0;
        }
    }

    public void _on_PlayerButton_button_down()
    {
        _isPressed = true;
    }

    public void _on_PlayerButton_button_up()
    {
        _isPressed = false;
    }
}
