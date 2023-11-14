using Godot;
using System;

public class MainMenu : Panel
{
	private GeneralSingleton _generalSingleton;
	private Label _maxWaveText;
	private Label _maxWaveValue;
	private Button _playButton;
	private Button _languageButton;
	private Button _exitButton;

	public override void _Ready()
	{
		_generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
		_generalSingleton.Connect("language_changed", this, nameof(_on_language_changed));
		_maxWaveText  = GetNode<Label>("MaxWave/Text");
		_maxWaveValue = GetNode<Label>("MaxWave/Value");
		_maxWaveValue.Text = $"{_generalSingleton.MaxWave}";
		

		_playButton = GetNode<Button>("VBoxContainer/PlayButton");
		_languageButton = GetNode<Button>("VBoxContainer/LanguageButton");
		_exitButton = GetNode<Button>("VBoxContainer/ExitButton");
		_on_language_changed(_generalSingleton.Language);
	}

	public void _on_language_changed(LanguageEnum languageNew)
	{
		DynamicFontData dynamicFontData;
		switch(languageNew)
		{
			case LanguageEnum.English:
				_maxWaveText.Text = "Max Wave:";
				_playButton.Text = "Play";
				_languageButton.Text = "Language";
				_exitButton.Text = "Exit";
				break;

			case LanguageEnum.Japanese:
				dynamicFontData = GD.Load<DynamicFontData>("res://Fonts/NotoSans/NotoSansJP-Regular.ttf");
				GeneralSingleton.ChangeFont(_maxWaveText, dynamicFontData);
				GeneralSingleton.ChangeFont(_playButton, dynamicFontData);
				GeneralSingleton.ChangeFont(_languageButton, dynamicFontData);
				GeneralSingleton.ChangeFont(_exitButton, dynamicFontData);

				_maxWaveText.Text = "最大ステージ:";
				_playButton.Text = "プレー";
				_languageButton.Text = "言語";
				_exitButton.Text = "エグジット";
				break;
			
			case LanguageEnum.Russian:
				_maxWaveText.Text = "Макс. волна:";
				_playButton.Text = "Играть";
				_languageButton.Text = "Язык";
				_exitButton.Text = "Выход";
				break;
			
			case LanguageEnum.Turkish:
				dynamicFontData = GD.Load<DynamicFontData>("res://Fonts/OpenSans/OpenSans-VariableFont_wdth,wght.ttf");
				GeneralSingleton.ChangeFont(_maxWaveText, dynamicFontData);
				GeneralSingleton.ChangeFont(_playButton, dynamicFontData);
				GeneralSingleton.ChangeFont(_languageButton, dynamicFontData);
				GeneralSingleton.ChangeFont(_exitButton, dynamicFontData);

				_maxWaveText.Text = "Maks. dalga:";
				_playButton.Text = "Oynamak";
				_languageButton.Text = "Dil";
				_exitButton.Text = "Çıkış";
				break;
		}
	}

	public void _on_PlayButton_pressed()
	{
		GetTree().ChangeScene("res://Main.tscn");
	}

	public void _on_ExitButton_pressed()
	{
		GetTree().Quit();
	}
}
