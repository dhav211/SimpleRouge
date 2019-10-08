using Godot;
using System;

public class Loot : Node2D
{
    Item itemContained;
    Vector2 gridPosition;

    public Item ItemContained
    {
        get { return itemContained; }
    }

    public override void _Ready()
    {
        
    }

    public void Initialize(Grid _grid, Item _item, Vector2 _position)
    {
        // Set a loot drop function in enemy. This will have chances, what it will have, etc
        // such grid variables such as position and occupant
        // set the item that the loot contains
        // set the transform for the item
        Position = _position;
        gridPosition = new Vector2(Mathf.FloorToInt(_position.x /16), Mathf.FloorToInt(_position.y /16));
        _grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].IsOccupied = true;
        _grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].Occupant = this;
        itemContained = _item;
    }

    public void PickUp(Player _player)
    {

    }
}
