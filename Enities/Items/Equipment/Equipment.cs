using Godot;
using System;

public class Equipment : Item
{
    public enum TypeOfEquipment { Weapon, Armor }
    [Export] public TypeOfEquipment SelectedTypeOfEquipment;
    [Export] public int Strength { get; set; }
    [Export] public int Defense { get; set; }
    public bool Equipped { get; set; }
}
