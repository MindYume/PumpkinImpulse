using Godot;
using System;

public class WavePanel : Control
{
    private GeneralSingleton _generalSingleton;
    private Label _labelText;
    private Label _labelValue;

    public override void _Ready()
    {
        _generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
        _labelText = GetNode<Label>("Text");
        _labelValue = GetNode<Label>("Value");

        switch(_generalSingleton.Language)
		{
			case LanguageEnum.Eng:
				_labelText.Text = "Wave: ";
				break;
			
			case LanguageEnum.Rus:
				_labelText.Text = "Волна: ";
				break;
		}
    }

    public void _on_Level_wave_value_changed(int newWaveValue)
    {
        _labelValue.Text = $"{newWaveValue}";
    }
}
