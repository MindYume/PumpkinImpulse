using Godot;
using System;

public class RestartPanel : Panel
{
    private GeneralSingleton _generalSingleton;
    private Label _completedWavesText;
    private Label _completedWavesValue;
    private Label _maxWaveText;
    private Label _maxWaveValue;
    private Button _menuButton;
    private Button _againButton;
    public override void _Ready()
    {
        _generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
        _completedWavesText  = GetNode<Label>("CompletedWaves/Text");
        _completedWavesValue = GetNode<Label>("CompletedWaves/Value");
        _maxWaveText  = GetNode<Label>("MaxWaves/Text");
        _maxWaveValue = GetNode<Label>("MaxWaves/Value");
        _menuButton = GetNode<Button>("HBoxContainer/MenuButton");
        _againButton = GetNode<Button>("HBoxContainer/AgainButton");

        setLanguage(_generalSingleton.Language);
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
        GetTree().ChangeScene("res://Main.tscn");
    }
    

    public void _on_Player_dead()
    {
        _maxWaveValue.Text = $"{_generalSingleton.MaxWave}";
        Show();
        GetTree().Paused = true;
    }
}
