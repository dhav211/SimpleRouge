using Godot;
using System;

public class Item : Node2D
{
    public enum ItemType { Potion, Key, Equipment }
    [Export] ItemType selectedTypeOfItem;
    [Export] string itemName;

    public ItemType SelectedTypeOfItem
    {
        get { return selectedTypeOfItem; }
        set { selectedTypeOfItem = value; }
    }

    public string ItemName
    {
        get { return itemName; }
        set { itemName = value; }
    }
}
