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

public class PausePanel : Panel
{
    private Label _label;
    private Button _playButton;
    private Button _menuButton;
    private Button _pauseButton;
    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
        _playButton = GetNode<Button>("HBoxContainer/PlayButton");
        _menuButton = GetNode<Button>("HBoxContainer/MainMenuButton");
        _pauseButton = GetParent().GetNode<Button>("PauseButton");

        setLanguage(GeneralSingleton.Instance.Language);
    }

    private void setLanguage(LanguageEnum language)
    {
        DynamicFontData dynamicFontData;
        switch(language)
		{
			case LanguageEnum.English:
				_label.Text = "Game paused";
                _playButton.Text = "Continue";
                _menuButton.Text = "Main menu";
                _pauseButton.Text = "Pause";
				break;
            
            case LanguageEnum.Japanese:
                dynamicFontData = GD.Load<DynamicFontData>("res://Fonts/NotoSans/NotoSansJP-Regular.ttf");
				GeneralSingleton.ChangeFont(_label, dynamicFontData);
                GeneralSingleton.ChangeFont(_playButton, dynamicFontData);
                GeneralSingleton.ChangeFont(_menuButton, dynamicFontData);
                GeneralSingleton.ChangeFont(_pauseButton, dynamicFontData);

				_label.Text = "休止";
                _playButton.Text = "ゲームを続ける";
                _menuButton.Text = "メインメニュー";
                _pauseButton.Text = "一時停止";
				break;
			
			case LanguageEnum.Russian:
				_label.Text = "Пауза";
                _playButton.Text = "Продолжить";
                _menuButton.Text = "Главное меню";
                _pauseButton.Text = "Пауза";
				break;
            
            case LanguageEnum.Turkish:
                dynamicFontData = GD.Load<DynamicFontData>("res://Fonts/OpenSans/OpenSans-VariableFont_wdth,wght.ttf");
				GeneralSingleton.ChangeFont(_label, dynamicFontData);
                GeneralSingleton.ChangeFont(_playButton, dynamicFontData);
                GeneralSingleton.ChangeFont(_menuButton, dynamicFontData);
                GeneralSingleton.ChangeFont(_pauseButton, dynamicFontData);

				_label.Text = "Duraklat";
                _playButton.Text = "Devam etmek";
                _menuButton.Text = "Ana menü";
                _pauseButton.Text = "Duraklat";
				break;
		}
    }

    public void _on_PauseButton_pressed()
    {
        Show();
        GetTree().Paused = true;
    }

    public void _on_PlayButton_pressed()
    {
        Hide();
        GetTree().Paused = false;
    }

    public void _on_MainMenuButton_pressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeScene("res://Objects/MainMenu.tscn");
    }
}
