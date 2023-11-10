using Godot;
using System;

public class PausePanel : Panel
{
    private GeneralSingleton _generalSingleton;
    private Label _label;
    private Button _playButton;
    private Button _menuButton;
    private Button _pauseButton;
    public override void _Ready()
    {
        _generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
        _label = GetNode<Label>("Label");
        _playButton = GetNode<Button>("HBoxContainer/PlayButton");
        _menuButton = GetNode<Button>("HBoxContainer/MainMenuButton");
        _pauseButton = GetParent().GetNode<Button>("PauseButton");

        setLanguage(_generalSingleton.Language);
    }

    private void setLanguage(LanguageEnum language)
    {
        switch(language)
		{
			case LanguageEnum.Eng:
				_label.Text = "Game paused";
                _playButton.Text = "Continue";
                _menuButton.Text = "Main menu";
                _pauseButton.Text = "Pause";
				break;
			
			case LanguageEnum.Rus:
				_label.Text = "Пауза";
                _playButton.Text = "Продолжить";
                _menuButton.Text = "Главное меню";
                _pauseButton.Text = "Пауза";
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
