using Godot;
using System;

public class LootDrop : Node2D
{
    [Export] PackedScene lootScene;
    [Export] PackedScene potion;
    [Export] PackedScene key;
    [Export] PackedScene equipment;
    [Export] int dropChance;
    Enemy enemy;

    public override void _Ready()
    {
        enemy = GetParent() as Enemy;
    }

    public void DropLoot(Random _random, Grid _grid)
    {
        int randomChance = _random.Next(0, 101);

        if (randomChance <= dropChance)
        {
            Loot loot = lootScene.Instance() as Loot;
            Item itemToDrop = SetItemToDrop(_random);
            loot.Initialize(_grid, itemToDrop, enemy.Position);
            GetTree().GetRoot().GetNode("Game").AddChild(loot);
        }
    }

    private Item SetItemToDrop(Random _random)
    {
        Item itemToDrop = null;

        int firstLevel = 70;
        int secondlevel = 90;

        int levelChance = _random.Next(0, 101);

        if (levelChance < firstLevel)
        {
            // drop potion
            itemToDrop = potion.Instance() as Item;
        }
        else if (levelChance >= firstLevel && levelChance < secondlevel)
        {
            // drop key
            itemToDrop = key.Instance() as Item;
        }
        else if (levelChance >= secondlevel)
        {
            // drop equipment
            itemToDrop = equipment.Instance() as Item;
        }

        return itemToDrop;
    }
}
