using Godot;
using System;

public class EquipmentHolder : Node2D
{
    Equipment weapon;
    Equipment armor;
    Player player;

    public Equipment Weapon
    {
        get { return weapon; }
    }

    public Equipment Armor
    {
        get { return armor; }
    }

    public override void _Ready()
    {
        player = GetParent() as Player;
    }

    public void Equip(Equipment _equipment)
    {
        if (_equipment.SelectedTypeOfEquipment == Equipment.TypeOfEquipment.Weapon)
        {
            if (weapon != null)
            {
                weapon.Equipped = false;
                player.Stats.Strength -= weapon.Strength;
            }
            weapon = _equipment;
            weapon.Equipped = true;
            player.Stats.Strength = player.Stats.BaseStrength + weapon.Strength;
            GetTree().CallGroup("PlayerInfo", "UpdatePlayerStrength");
        }
        else if (_equipment.SelectedTypeOfEquipment == Equipment.TypeOfEquipment.Armor)
        {
            if (armor != null)
            {
                armor.Equipped = false;
                player.Stats.Defense -= armor.Defense;
            }
            armor = _equipment;
            armor.Equipped = true;
            player.Stats.Defense = player.Stats.BaseDefense + armor.Defense;
            GetTree().CallGroup("PlayerInfo", "UpdatePlayerDefense");
        }
    }
}
