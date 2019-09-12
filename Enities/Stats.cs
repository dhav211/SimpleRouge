using Godot;
using System;

public class Stats : Node2D
{
    [Export] public int Health { get; set; }
    [Export] public int Strength { get; set; }
    [Export] public int Defense { get; set; }
    public int CurrentHealth { get; set; }

    public override void _Ready()
    {
        CurrentHealth = Health;
    }

    public void CheckIfCurrentHealthIsInBound()
    {
        // If a player goes below 0 HP or above max HP, then correct it here so it isn't seen in UI.

        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
        else if (CurrentHealth > Health)
        {
            CurrentHealth = Health;
        }
    }
}
