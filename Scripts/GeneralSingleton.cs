using Godot;
using System.Collections.Generic;

public enum LanguageEnum
{
    English,
    Japanese,
    Russian,
    Turkish
}

public class GeneralSingleton : Node
{
    private Player _player;
    public Node2D BulletsContainer;
    private LanguageEnum _language = LanguageEnum.English;
    public int MaxWave = 0;
    [Signal] delegate void language_changed(LanguageEnum languageNew);

    public LanguageEnum Language
    {
        get => _language;
        set
        {
            _language = value;
            EmitSignal(nameof(language_changed), value);
        }
    }

    public Player PlayerNode
    {
        set => _player = value;
        get => _player;
    }

    public void PlaySound(string soundName, float volumeDb, float pitchScale)
    {
        AudioStreamPlayer soundPlayer = new AudioStreamPlayer();
        soundPlayer.Stream = GD.Load<AudioStream>($"res://Sounds/{soundName}.wav");
        AddChild(soundPlayer);
        soundPlayer.VolumeDb = volumeDb;
        soundPlayer.PitchScale = pitchScale;
        soundPlayer.Connect("finished", soundPlayer, "queue_free");
        soundPlayer.Play();
    }

    public static void ChangeFont(Control control, DynamicFontData dynamicFontData)
	{
        DynamicFont dynamicFontNew = new DynamicFont
        {
            FontData = dynamicFontData
        };
		DynamicFont dynamicFontControl = ((DynamicFont)control.GetFont("font"));
		dynamicFontNew.Size = dynamicFontControl.Size;
		dynamicFontNew.OutlineSize = dynamicFontControl.OutlineSize;
		dynamicFontNew.OutlineColor = dynamicFontControl.OutlineColor;
		dynamicFontNew.ExtraSpacingTop = dynamicFontControl.ExtraSpacingTop;
		dynamicFontNew.ExtraSpacingBottom = dynamicFontControl.ExtraSpacingBottom;

        control.AddFontOverride("font", dynamicFontNew);
	}
}
