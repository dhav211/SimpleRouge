using Godot;
using System;

public class ConsumableItem : Item
{
    public override void _Ready()
    {
        
    }

    public int GetRestoreAmount(Player _player)
    {
        return Mathf.FloorToInt(_player.Stats.Health * .25f);
    }
}
