using Godot;
using System;

public interface IEnemy
{
    float Health {set; get;}
    void TakeDamage(float damage_value);
}

