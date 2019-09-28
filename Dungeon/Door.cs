using Godot;
using System;

public class Door : Node2D
{
    public enum LockState { Unlocked, Locked }
    LockState currentLockState = LockState.Unlocked;
    public Vector2 gridPosition = new Vector2();
    public Key KeyRequired {get; set;}

    Sprite sprite;
    Grid grid;

    public override void _Ready()
    {
        sprite = GetNode("Sprite") as Sprite;
        //grid = GetTree().GetRoot().GetNode("Game/Grid") as Grid;  // TODO add this back in later as time of dungeon generation

        /*
        gridPosition = new Vector2(Mathf.FloorToInt(Position.x /16), Mathf.FloorToInt(Position.y /16));
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].IsOccupied = true;
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].Occupant = this;
        */
    }

    public void InitializeDoor(Grid _grid, LockState _lockState)  // TEMP METHOD!!
    {
        grid = _grid;

        Position = new Vector2(32, 32);
        gridPosition = new Vector2(Mathf.FloorToInt(Position.x /16), Mathf.FloorToInt(Position.y /16));
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].IsOccupied = true;
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].Occupant = this;

        currentLockState = _lockState;
    }

    public void OpenDoor(Player _player)
    {
        if (currentLockState == LockState.Unlocked)
        {
            grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].IsOccupied = false;
            grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].Occupant = null;
            QueueFree();
        }
        else if (currentLockState == LockState.Locked)
        {
            GD.Print("Door is locked with " + KeyRequired.ItemName);
            // Search thru players inventory and see if it has required key to open.
            // If player has key, then unlock.
            // TODO When console functions, print that the required key is needed to open door

            if (_player.Inventory.IsItemInIventory(KeyRequired))
            {
                grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].IsOccupied = false;
                grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].Occupant = null;
                _player.Inventory.RemoveItem(KeyRequired);
                QueueFree();
            }
        }
    }
}
