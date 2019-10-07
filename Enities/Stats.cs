using Godot;
using System;

public class Stats : Node2D
{
    [Export] public string EntityName { get; set; }
    [Export] public int Health { get; set; }
    [Export] public int Strength { get; set; }
    [Export] public int BaseStrength { get; set; }
    [Export] public int Defense { get; set; }
    [Export] public int BaseDefense { get; set; }
    [Export] public int Level { get; set; }
    int currentHealth; 
    public int CurrentHealth
    {
        get { return currentHealth; }
        set 
        {
            currentHealth = value;
            CheckIfCurrentHealthIsInBound();
        }
    }

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

    public void CallForUpdateOfPlayerCurrentHP()
    {
        GetTree().CallGroup("PlayerInfo", "UpdatePlayerCurrentHP");
    }
}
