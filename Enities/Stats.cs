using Godot;
using System;

public class Stats : Node2D
{
    [Export] public int Health { get; set; }
    [Export] public int Strength { get; set; }
    [Export] public int Defense { get; set; }
    public int CurrentHealth { get; set; }
}
