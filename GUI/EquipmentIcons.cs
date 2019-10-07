using Godot;
using System;

public class EquipmentIcons : NinePatchRect
{
    EquipMenu equipMenu;

    public override void _Ready()
    {
        equipMenu = GetTree().GetRoot().GetNode("Game/CanvasLayer/GUI/EquipMenu") as EquipMenu;
    }

    public void _on_WeaponButton_pressed()
    {
        equipMenu.OpenMenu(Equipment.TypeOfEquipment.Weapon);
    }

    public void _on_ArmorButton_pressed()
    {
        equipMenu.OpenMenu(Equipment.TypeOfEquipment.Armor);
    }
}
