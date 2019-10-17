using Godot;
using System;

public class Door : Node2D
{
    public enum LockState { Unlocked, Locked }
    LockState currentLockState = LockState.Unlocked;
    public Vector2 gridPosition = new Vector2();
    Vector2 positionToSet;
    public Key KeyRequired {get; set;}

    Sprite sprite;
    Grid grid;
    Console console;

    public override void _Ready()
    {
        sprite = GetNode("Sprite") as Sprite;
        console = GetTree().GetRoot().GetNode("Game/CanvasLayer/GUI/Console") as Console;
        Position = positionToSet;
    }

    public void InitializeDoor(Grid _grid, LockState _lockState, Vector2 _position, Key _keyRequired)
    {
        grid = _grid;

        positionToSet = _position;
        gridPosition = new Vector2(Mathf.FloorToInt(positionToSet.x /16), Mathf.FloorToInt(positionToSet.y /16));
        KeyRequired = _keyRequired;
        currentLockState = _lockState;
    }

    public void SetDoor()
    {
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].IsOccupied = true;
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].Occupant = this;
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
            // Search thru players inventory and see if it has required key to open.
            // If player has key, then unlock.

            if (_player.Inventory.IsItemInIventory(KeyRequired))
            {
                grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].IsOccupied = false;
                grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].Occupant = null;
                _player.Inventory.RemoveItem(KeyRequired);
                console.PrintMessageToConsole("Door was unlocked with " + KeyRequired.ItemName);
                QueueFree();
            }
            else  // Player doesn't have the key to unlock door
            {
                console.PrintMessageToConsole("Door is locked with " + KeyRequired.ItemName);
            }
        }
    }
}
