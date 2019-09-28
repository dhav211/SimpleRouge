using Godot;
using System;

public class Chest : Node2D
{
    enum OpenState { Opened, Closed }
    OpenState currentOpenState = OpenState.Closed;
    Vector2 gridPosition;
    Rect2 closedRegion = new Rect2(170, 102, 16, 16);
    Rect2 openRegion = new Rect2(187, 102, 16, 16);
    Item itemInside;

    Grid grid;
    Sprite sprite;

    public override void _Ready()
    {
        sprite = GetNode("Sprite") as Sprite;
        SetSprite();
    }

    public void InitializeChest(Grid _grid, Item _itemInside)
    {
        grid = _grid;

        Position = new Vector2(64, 64);
        gridPosition = new Vector2(Mathf.FloorToInt(Position.x /16), Mathf.FloorToInt(Position.y /16));
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].IsOccupied = true;
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].Occupant = this;
        itemInside = _itemInside;
    }

    public void OpenChest(Player _player)
    {
        if (currentOpenState == OpenState.Closed)
        {
            currentOpenState = OpenState.Opened;
            SetSprite();
            _player.Inventory.AddItem(itemInside);
        }
        else if (currentOpenState == OpenState.Opened)
        {
            GD.Print("Chest has already been opened.");
            // TODO print this message to the console
        }
    }

    private void SetSprite()
    {
        if (currentOpenState == OpenState.Closed)
        {
            sprite.RegionRect = closedRegion;
        }
        else if (currentOpenState == OpenState.Opened)
        {
            sprite.RegionRect = openRegion;
        }
    }
}
