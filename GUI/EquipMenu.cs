using Godot;
using System;

public class EquipMenu : NinePatchRect
{
    [Export] PackedScene equipmentButtonScene;
    VBoxContainer equipmentContainer;
    Label currentlyEquipped;
    Label strengthText;
    Label defenseText;
    Player player;
    public int PlayerStrength { get; set; }
    public int PlayerDefense { get; set; }

    public Label StrengthText
    {
        get { return strengthText; }
        set { strengthText = value; }
    }

    public Label DefenseText
    {
        get { return strengthText; }
        set { defenseText = value; }
    }

    public override void _Ready()
    {
        player = GetTree().GetRoot().GetNode("Game/Player") as Player;
        currentlyEquipped = GetNode("CurrentlyEquippedContainer/Equipment") as Label;
        equipmentContainer = GetNode("ScrollContainer/EquipmentContainer") as VBoxContainer;
        strengthText = GetNode("StatBox/VBoxContainer/STRContainer/CurrentSTR") as Label;
        defenseText = GetNode("StatBox/VBoxContainer/DEFContainer/CurrentDEF") as Label;

        Visible = false;
    }

    public void OpenMenu(Equipment.TypeOfEquipment _type)
    {
        Visible = true;
        // Set player stat info back to default
        PlayerStrength = player.Stats.Strength;
        PlayerDefense = player.Stats.Defense;
        strengthText.Text = PlayerStrength.ToString();
        defenseText.Text = PlayerDefense.ToString();
        strengthText.AddColorOverride("font_color", Color.ColorN("White"));
        defenseText.AddColorOverride("font_color", Color.ColorN("White"));

        if (_type == Equipment.TypeOfEquipment.Weapon)
        {
            currentlyEquipped.Text = player.Equipment.Weapon.ItemName;
        }
        else if (_type == Equipment.TypeOfEquipment.Armor)
        {
            currentlyEquipped.Text = player.Equipment.Armor.ItemName;
        }

        AddEquipmentButtons(_type);
    }

    private void AddEquipmentButtons(Equipment.TypeOfEquipment _type)
    {
        // Adds all avaliable equipment of type to equip into the scroll container.

        foreach (Item item in player.Inventory.Items)
        {
            if (item.SelectedTypeOfItem == Item.ItemType.Equipment)
            {
                Equipment equipment = item as Equipment;  // If the item is equipment, it needs to be casted to the equipment class for further use

                if (!equipment.Equipped && equipment.SelectedTypeOfEquipment == _type)  // Items already equipped won't show up in the scroll container
                {
                    EquipmentButton equipmentButton = equipmentButtonScene.Instance() as EquipmentButton;
                    equipmentButton.InitializeButton(equipment, this, player);
                    equipmentContainer.AddChild(equipmentButton);
                }
            }
        }
    }

    public void _on_CloseButton_pressed()
    {
        CloseMenu();
    }

    public void CloseMenu()
    {
        Godot.Collections.Array equipmentButtons = equipmentContainer.GetChildren();

        foreach (Button child in equipmentButtons)
        {
            child.QueueFree();
        }

        Visible = false;
    }
}
