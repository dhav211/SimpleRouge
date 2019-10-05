using Godot;
using System;

public class ItemContainer : VBoxContainer
{
    Player player;
    Label potionLabel;
    Label simpleKeyLabel;
    Label masterKeyLabel;

    public override void _Ready()
    {
        player = GetTree().GetRoot().GetNode("Game/Player") as Player;
        potionLabel = GetNode("Potions/Amount") as Label;
        simpleKeyLabel = GetNode("SimpleKeys/Amount") as Label;
        masterKeyLabel = GetNode("MasterKeys/Amount") as Label;

        InitializeItemAmounts();
    }

    public void UpdateItemContainer(Item _item)
    {
        int amount = GetAmountOfItem(_item);
        SetAmountText(_item, amount);
    }

    private int GetAmountOfItem(Item _item)
    {
        int itemAmount = 0;

        foreach (Item item in player.Inventory.Items)
        {
            if (item.Name == _item.Name)
                itemAmount++;
        }

        return itemAmount;
    }

    private void SetAmountText(Item _item, int _amount)
    {
        if (_item.Name == "Potion")
        {
            potionLabel.Text = _amount.ToString();
        }
        else if (_item.Name == "SilverKey")
        {
            simpleKeyLabel.Text = _amount.ToString();
        }
        else if (_item.Name == "MasterKey")
        {
            masterKeyLabel.Text = _amount.ToString();
        }
    }

    private void InitializeItemAmounts()
    {
        string[] itemsToCheck = {"Potion", "SilverKey", "MasterKey"};

        if (player.Inventory.Items.Count != 0)
        {
            for (int i = 0; i < itemsToCheck.Length; ++i)
            {
                int itemAmount = 0;
                Item itemToCheck = null;

                foreach (Item item in player.Inventory.Items)
                {
                    if (item.Name == itemsToCheck[i])
                    {
                        itemAmount++;

                        if (itemToCheck == null)
                            itemToCheck = item;
                    }
                }

                if (itemAmount > 0)
                    SetAmountText(itemToCheck, itemAmount);
            }
        }
    }
}
