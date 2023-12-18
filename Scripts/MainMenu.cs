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

public class MainMenu : Panel
{
	private Label _maxWaveText;
	private Label _maxWaveValue;
	private Button _playButton;
	private Button _languageButton;
	private Button _exitButton;

	public override void _Ready()
	{
		GeneralSingleton.Instance.OnlanguageChanged += _on_language_changed;
		_maxWaveText  = GetNode<Label>("MaxWave/Text");
		_maxWaveValue = GetNode<Label>("MaxWave/Value");
		_maxWaveValue.Text = $"{GeneralSingleton.Instance.MaxWave}";
		

		_playButton = GetNode<Button>("VBoxContainer/PlayButton");
		_languageButton = GetNode<Button>("VBoxContainer/LanguageButton");
		_exitButton = GetNode<Button>("VBoxContainer/ExitButton");
		_on_language_changed(GeneralSingleton.Instance.Language);
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
		GetTree().ChangeScene("res://Objects/Main.tscn");
	}

	public void _on_ExitButton_pressed()
	{
		GetTree().Quit();
	}

	public void _on_MainMenu_tree_exiting()
	{
		GD.Print("Main menu exit");
		GeneralSingleton.Instance.OnlanguageChanged -= _on_language_changed;
	}
}
