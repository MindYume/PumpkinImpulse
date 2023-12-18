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

public class WavePanel : Control
{
    private Label _labelText;
    private Label _labelValue;

    public override void _Ready()
    {

        _labelText = GetNode<Label>("Text");
        _labelValue = GetNode<Label>("Value");

        switch(GeneralSingleton.Instance.Language)
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
