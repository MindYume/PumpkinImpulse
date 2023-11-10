using Godot;
using System;

public class ButtonSounds : Node
{
    private GeneralSingleton _generalSingleton;
    public override void _Ready()
    {
        _generalSingleton = GetTree().Root.GetNode<GeneralSingleton>("GeneralSingleton");
        Node parent = GetParent();
        if (parent is Control)
        {
            parent.Connect("mouse_entered", this, nameof(_on_mouse_entered));
            parent.Connect("pressed", this, nameof(_on_pressed));
        }
    }

    public void _on_mouse_entered()
    {
        _generalSingleton.PlaySound("btn_hover", 0, 1);
    }

    public void _on_pressed()
    {
        _generalSingleton.PlaySound("btn_click", -10, 1);
    }
}
