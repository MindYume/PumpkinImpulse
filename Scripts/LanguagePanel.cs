using Godot;
using System;

public class LanguagePanel : Panel
{
    private GeneralSingleton _generalSingleton;
    private CheckBox _checkBoxEnglish;
    private CheckBox _checkBoxJapanese;
    private CheckBox _checkBoxRussian;
    private CheckBox _checkBoxTurkish;

    public override void _Ready()
    {
        _generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
        _checkBoxEnglish  = GetNode<CheckBox>("VBoxContainer/English/CheckBox");
        _checkBoxJapanese = GetNode<CheckBox>("VBoxContainer/Japanese/CheckBox");
        _checkBoxRussian  = GetNode<CheckBox>("VBoxContainer/Russian/CheckBox");
        _checkBoxTurkish  = GetNode<CheckBox>("VBoxContainer/Turkish/CheckBox");

        switch(_generalSingleton.Language)
		{
			case LanguageEnum.English:
				_on_English_pressed();
				break;

            case LanguageEnum.Japanese:
				_on_Japanese_pressed();
				break;
			
			case LanguageEnum.Russian:
				_on_Russian_pressed();
				break;

            case LanguageEnum.Turkish:
				_on_Turkish_pressed();
				break;
		}
    }
    
    public void _on_LanguageButton_pressed()
    {
        Show();
    }

    public void _on_CloseButton_pressed()
    {
        Hide();
    }

    public void _on_English_pressed()
    {
        _generalSingleton.Language = LanguageEnum.English;
        _checkBoxEnglish.Pressed  = true;
        _checkBoxJapanese.Pressed = false;
        _checkBoxRussian.Pressed  = false;
        _checkBoxTurkish.Pressed  = false;
    }

    public void _on_Japanese_pressed()
    {
        _generalSingleton.Language = LanguageEnum.Japanese;
        _checkBoxEnglish.Pressed  = false;
        _checkBoxJapanese.Pressed = true;
        _checkBoxRussian.Pressed  = false;
        _checkBoxTurkish.Pressed  = false;
    }

    public void _on_Russian_pressed()
    {
        _generalSingleton.Language = LanguageEnum.Russian;
        _checkBoxEnglish.Pressed  = false;
        _checkBoxJapanese.Pressed = false;
        _checkBoxRussian.Pressed  = true;
        _checkBoxTurkish.Pressed  = false;
    }
    public void _on_Turkish_pressed()
    {
        _generalSingleton.Language = LanguageEnum.Turkish;
        _checkBoxEnglish.Pressed  = false;
        _checkBoxJapanese.Pressed = false;
        _checkBoxRussian.Pressed  = false;
        _checkBoxTurkish.Pressed  = true;
    }
}
