using Godot;
using System;

public class Chest : Node2D
{
    enum OpenState { Opened, Closed }
    OpenState currentOpenState = OpenState.Closed;
    Vector2 gridPosition;
    Vector2 positionToSet;
    Rect2 closedRegion = new Rect2(170, 102, 16, 16);
    Rect2 openRegion = new Rect2(187, 102, 16, 16);
    Item itemInside;

    Grid grid;
    Sprite sprite;

    public override void _Ready()
    {
        sprite = GetNode("Sprite") as Sprite;
        SetSprite();
        Position = positionToSet;
    }

    public void InitializeChest(Grid _grid, Item _itemInside, Vector2 _position)
    {
        grid = _grid;

        positionToSet = _position;
        gridPosition = new Vector2(Mathf.FloorToInt(positionToSet.x /16), Mathf.FloorToInt(positionToSet.y /16));
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
