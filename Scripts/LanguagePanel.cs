using Godot;
using System;

public class LanguagePanel : Panel
{
    private GeneralSingleton _generalSingleton;
    private CheckBox _checkBoxEng;
    private CheckBox _checkBoxRus;

    public override void _Ready()
    {
        _generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
        _checkBoxEng = GetNode<CheckBox>("VBoxContainer/English/CheckBox");
        _checkBoxRus = GetNode<CheckBox>("VBoxContainer/Russian/CheckBox");

        switch(_generalSingleton.Language)
		{
			case LanguageEnum.Eng:
				_checkBoxEng.Pressed = true;
                _checkBoxRus.Pressed = false;
				break;
			
			case LanguageEnum.Rus:
				_checkBoxEng.Pressed = false;
                _checkBoxRus.Pressed = true;
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
        _generalSingleton.Language = LanguageEnum.Eng;
        _checkBoxEng.Pressed = true;
        _checkBoxRus.Pressed = false;
    }

    public void _on_Russian_pressed()
    {
        _generalSingleton.Language = LanguageEnum.Rus;
        _checkBoxEng.Pressed = false;
        _checkBoxRus.Pressed = true;
    }
}
