using Godot;
using System;
using System.Collections.Generic;

public class Inventory : Node2D
{
    List<Item> items = new List<Item>();

    public List<Item> Items
    {
        get { return items; }
        set { items = value; }
    }

    public override void _Ready()
    {
        
    }

    public void AddItem(Item _itemToAdd)
    {
        GD.Print(_itemToAdd.ItemName + " has been added to inventory"); // TODO print this in game console
        items.Add(_itemToAdd);
    }

    public void RemoveItem(Item _itemToRemove)
    {
        foreach(Item item in items)
        {
            if (item == _itemToRemove)
            {
                items.Remove(_itemToRemove);
                GD.Print(_itemToRemove.ItemName + " has been removed from inventory"); // TODO print this in game console
                break;
            }
        }
    }

    public bool IsItemInIventory(Item _itemToCheck)
    {
        foreach (Item item in items)
        {
            if (item == _itemToCheck)
            {
                GD.Print(_itemToCheck.ItemName + " is in inventory");
                return true;
            }
        }

        return false;
    }
}
