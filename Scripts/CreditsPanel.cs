using Godot;
using System;

public class CreditsPanel : Panel
{
    public void _on_CreditsButton_pressed()
    {
        Show();
    }

    public void _on_CloseButton_pressed()
    {
        Hide();
    }
}
