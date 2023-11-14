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
			case LanguageEnum.English:
				_labelText.Text = "Wave: ";
				break;

            case LanguageEnum.Japanese:
                DynamicFontData dynamicFontData = GD.Load<DynamicFontData>("res://Fonts/NotoSans/NotoSansJP-Regular.ttf");
				GeneralSingleton.ChangeFont(_labelText, dynamicFontData);

				_labelText.Text = "ステージ: ";
				break;
			
			case LanguageEnum.Russian:
				_labelText.Text = "Волна: ";
				break;
            
            case LanguageEnum.Turkish:
				_labelText.Text = "Dalga: ";
				break;
		}
    }

    public void _on_Level_wave_value_changed(int newWaveValue)
    {
        _labelValue.Text = $"{newWaveValue}";
    }
}
