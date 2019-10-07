using Godot;
using System;

public class EquipmentButton : Button
{
    Equipment equipmentToEquip;
    EquipMenu equipMenu;
    Player player;

    public void InitializeButton(Equipment _equipment, EquipMenu _equipMenu, Player _player)
    {
        Text = _equipment.ItemName;
        equipmentToEquip = _equipment;
        equipMenu = _equipMenu;
        player = _player;
    }

    public void _on_EquipmentButton_pressed()
    {
        player.Equipment.Equip(equipmentToEquip);
        equipMenu.CloseMenu();
    }

    public void _on_EquipmentButton_mouse_entered()
    {
        // Change stats in equip menu to what stats would be with equipment equipped
        if (equipmentToEquip.SelectedTypeOfEquipment == Equipment.TypeOfEquipment.Weapon)
        {
            int newStrength = player.Stats.BaseStrength + equipmentToEquip.Strength;
            equipMenu.StrengthText.Text = newStrength.ToString();

            SetStatColor(newStrength, player.Stats.Strength, equipMenu.StrengthText);
        }
        else if (equipmentToEquip.SelectedTypeOfEquipment == Equipment.TypeOfEquipment.Armor)
        {
            int newDefense = player.Stats.BaseDefense + equipmentToEquip.Defense;
            equipMenu.DefenseText.Text = newDefense.ToString();

            SetStatColor(newDefense, player.Stats.Defense, equipMenu.DefenseText);
        }
    }

    private void SetStatColor(int _newStat, int _oldStat, Label _statText)
    {
        if (_newStat > _oldStat)
        {
            _statText.AddColorOverride("font_color", Color.ColorN("Green"));
        }
        else if (_newStat < _oldStat)
        {
            _statText.AddColorOverride("font_color", Color.ColorN("Red"));
        }
    }

    public void _on_EquipmentButton_mouse_exited()
    {
        // set stats back to defualt in equip menu
        equipMenu.StrengthText.AddColorOverride("font_color", Color.ColorN("White"));
        equipMenu.DefenseText.AddColorOverride("font_color", Color.ColorN("White"));
        equipMenu.StrengthText.Text = player.Stats.Strength.ToString();
        equipMenu.DefenseText.Text = player.Stats.Defense.ToString();
    }
}
