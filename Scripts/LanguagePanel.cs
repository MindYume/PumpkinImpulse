/*
MIT License

Copyright (c) 2023 Viktor Grachev

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using Godot;

public class LanguagePanel : Panel
{
    private CheckBox _checkBoxEnglish;
    private CheckBox _checkBoxJapanese;
    private CheckBox _checkBoxRussian;
    private CheckBox _checkBoxTurkish;

    public override void _Ready()
    {
        _checkBoxEnglish  = GetNode<CheckBox>("VBoxContainer/English/CheckBox");
        _checkBoxJapanese = GetNode<CheckBox>("VBoxContainer/Japanese/CheckBox");
        _checkBoxRussian  = GetNode<CheckBox>("VBoxContainer/Russian/CheckBox");
        _checkBoxTurkish  = GetNode<CheckBox>("VBoxContainer/Turkish/CheckBox");

        switch(GeneralSingleton.Instance.Language)
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
        GeneralSingleton.Instance.Language = LanguageEnum.English;
        _checkBoxEnglish.Pressed  = true;
        _checkBoxJapanese.Pressed = false;
        _checkBoxRussian.Pressed  = false;
        _checkBoxTurkish.Pressed  = false;
    }

    public void _on_Japanese_pressed()
    {
        GeneralSingleton.Instance.Language = LanguageEnum.Japanese;
        _checkBoxEnglish.Pressed  = false;
        _checkBoxJapanese.Pressed = true;
        _checkBoxRussian.Pressed  = false;
        _checkBoxTurkish.Pressed  = false;
    }

    public void _on_Russian_pressed()
    {
        GeneralSingleton.Instance.Language = LanguageEnum.Russian;
        _checkBoxEnglish.Pressed  = false;
        _checkBoxJapanese.Pressed = false;
        _checkBoxRussian.Pressed  = true;
        _checkBoxTurkish.Pressed  = false;
    }
    public void _on_Turkish_pressed()
    {
        GeneralSingleton.Instance.Language = LanguageEnum.Turkish;
        _checkBoxEnglish.Pressed  = false;
        _checkBoxJapanese.Pressed = false;
        _checkBoxRussian.Pressed  = false;
        _checkBoxTurkish.Pressed  = true;
    }
}
