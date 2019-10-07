using Godot;
using System;
using System.Collections.Generic;

public class Inventory : Node2D
{
    List<Item> items = new List<Item>();
    Console console;
    ItemContainer itemContainer;

    public List<Item> Items
    {
        get { return items; }
        set { items = value; }
    }

    public override void _Ready()
    {
        console = GetTree().GetRoot().GetNode("Game/CanvasLayer/GUI/Console") as Console;
        itemContainer = GetTree().GetRoot().GetNode("Game/CanvasLayer/GUI/SideMenu/Items/ItemContainer") as ItemContainer;
    }

    public void AddItem(Item _itemToAdd)
    {
        items.Add(_itemToAdd);

        if (_itemToAdd.SelectedTypeOfItem == Item.ItemType.Key || _itemToAdd.SelectedTypeOfItem == Item.ItemType.Potion)
        {
            itemContainer.UpdateItemContainer(_itemToAdd);
        }
    }

    public void RemoveItem(Item _itemToRemove)
    {
        foreach(Item item in items)
        {
            if (item.Name == _itemToRemove.Name)
            {
                items.Remove(item);
                if (_itemToRemove.SelectedTypeOfItem == Item.ItemType.Key || _itemToRemove.SelectedTypeOfItem == Item.ItemType.Potion)
                {
                    itemContainer.UpdateItemContainer(_itemToRemove);
                }
                break;
            }
        }
    }

    public bool IsItemInIventory(Item _itemToCheck)
    {
        foreach (Item item in items)
        {
            if (item.Name == _itemToCheck.Name)
            {
                return true;
            }
        }

        return false;
    }
}
