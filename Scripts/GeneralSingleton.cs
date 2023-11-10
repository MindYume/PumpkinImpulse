using Godot;
using System.Collections.Generic;

public enum LanguageEnum
{
    Eng,
    Rus
}

public class GeneralSingleton : Node
{
    private Player _player;
    public Node2D BulletsContainer;
    private LanguageEnum _language = LanguageEnum.Eng;
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
}
