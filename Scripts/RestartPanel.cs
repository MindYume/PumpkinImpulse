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

public class RestartPanel : Panel
{
    private Label _completedWavesText;
    private Label _completedWavesValue;
    private Label _maxWaveText;
    private Label _maxWaveValue;
    private Button _menuButton;
    private Button _againButton;
    public override void _Ready()
    {
        _completedWavesText  = GetNode<Label>("CompletedWaves/Text");
        _completedWavesValue = GetNode<Label>("CompletedWaves/Value");
        _maxWaveText  = GetNode<Label>("MaxWaves/Text");
        _maxWaveValue = GetNode<Label>("MaxWaves/Value");
        _menuButton = GetNode<Button>("HBoxContainer/MenuButton");
        _againButton = GetNode<Button>("HBoxContainer/AgainButton");

        setLanguage(GeneralSingleton.Instance.Language);
    }

    private void setLanguage(LanguageEnum language)
    {
        DynamicFontData dynamicFontData;
        switch(language)
		{
			case LanguageEnum.English:
				_maxWaveText.Text = "Max wave: ";
                _completedWavesText.Text = "Completed waves: ";
                _menuButton.Text = "Menu";
                _againButton.Text = "Again";
				break;
            
            case LanguageEnum.Japanese:
                dynamicFontData = GD.Load<DynamicFontData>("res://Fonts/NotoSans/NotoSansJP-Regular.ttf");
				GeneralSingleton.ChangeFont(_maxWaveText, dynamicFontData);
                GeneralSingleton.ChangeFont(_completedWavesText, dynamicFontData);
                GeneralSingleton.ChangeFont(_menuButton, dynamicFontData);
                GeneralSingleton.ChangeFont(_againButton, dynamicFontData);

				_maxWaveText.Text = "最大ステージ: ";
                _completedWavesText.Text = "完了したステージ: ";
                _menuButton.Text = "メニュー";
                _againButton.Text = "再びプレー";
				break;
			
			case LanguageEnum.Russian:
				_maxWaveText.Text = "Максимальная волна: ";
                _completedWavesText.Text = "Пройденые волны: ";
                _menuButton.Text = "Меню";
                _againButton.Text = "Играть снова";
				break;
            
            case LanguageEnum.Turkish:
                dynamicFontData = GD.Load<DynamicFontData>("res://Fonts/OpenSans/OpenSans-VariableFont_wdth,wght.ttf");
                GeneralSingleton.ChangeFont(_maxWaveText, dynamicFontData);
                GeneralSingleton.ChangeFont(_completedWavesText, dynamicFontData);
                GeneralSingleton.ChangeFont(_menuButton, dynamicFontData);
                GeneralSingleton.ChangeFont(_againButton, dynamicFontData);

				_maxWaveText.Text = "Maks. dalga: ";
                _completedWavesText.Text = "Tamamlanmış dalgalar: ";
                _menuButton.Text = "Menü";
                _againButton.Text = "Tekrar";
				break;
		}
    }

    public void _on_Level_wave_value_changed(int newWaveValue)
    {
        _completedWavesValue.Text = $"{newWaveValue-1}";
    }

    public void _on_MenuButton_pressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeScene("res://Objects/MainMenu.tscn");
    }

    public void _on_AgainButton_pressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeScene("res://Objects/Main.tscn");
    }
    

    public void _on_Player_dead()
    {
        _maxWaveValue.Text = $"{GeneralSingleton.Instance.MaxWave}";
        Show();
        GetTree().Paused = true;
    }
}
