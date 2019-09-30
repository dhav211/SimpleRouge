using Godot;
using System;

public class PlayerInput : Node2D
{
    Vector2 moveToPosition;
    Player player;
    TurnManager turnManager;
    MovementCursor movementCursor;
    Grid grid;
    Attack attack = new Attack();

    public override void _Ready()
    {
        player = GetParent() as Player;
        turnManager =  GetNode("/root/TurnManager") as TurnManager;
        grid = GetTree().GetRoot().GetNode("Game/Grid") as Grid;
        movementCursor = GetParent().GetNode("MovementCursor") as MovementCursor;
    }

    public override void _Input(InputEvent @event)
    {
        if (turnManager.CurrentTurn == player)
        {
            if (@event is InputEventMouseButton mouseInput)
            {
                HandleMouseInput(mouseInput);
            }
            if (@event is InputEventKey keyboardInput)
            {
                HandleKeyboardInput(keyboardInput);
            }
        }
    }

    private void HandleMouseInput(InputEventMouseButton _mouseInput)
    {
        // Handles state of the mouse click. Move to position is based upon direction of the MoveCursor script, so this might seem a bit strange.
        // A more effective way wouldn't probably just get the mouse position vector here, do the math and determine the direction from that, but the movement
            // cursor script was already there doing that work, so why not.

        Vector2 positionToMove = player.GridPosition + movementCursor.MoveToPosition;

        if (_mouseInput.IsActionPressed("ui_left_click"))
        {
            if (IsPositionWalkable(positionToMove))
            {
                if (!IsPositionOccupied(positionToMove))
                {
                    MovePlayer(positionToMove);
                }
                else  // Position is occupied
                {
                    InteractWithTileOccupant(positionToMove);
                    turnManager.EmitSignal("turn_completed");
                }
            }
            else if (!IsPositionWalkable(positionToMove) || movementCursor.MoveToPosition == new Vector2(0,0))  // In valid turn, skip turn
            {
                turnManager.EmitSignal("turn_completed");
            }
        }
    }

    private void HandleKeyboardInput(InputEventKey _keyboardInput)
    {
        // More or less that exact same as mouse movement, but with keyboard and doesn't rely on movement cursor.

        Vector2 positionToMove = GetKeyboardMoveToPosition(_keyboardInput);
        if (IsPositionWalkable(positionToMove))
        {
            if (!IsPositionOccupied(positionToMove))
            {
                MovePlayer(positionToMove);
            }
            else  // Position is occupied
            {
                InteractWithTileOccupant(positionToMove);
                turnManager.EmitSignal("turn_completed");
            }
        }
    }

    private Vector2 GetKeyboardMoveToPosition(InputEventKey _keyboardInput)
    {
        // Returns a vector2 position which the player will move based upon directions pressed on keyboard.
        // The keyboard input will return a simple direction which will be added to the players current grid position

        Vector2 moveToPosition = new Vector2();

        // Cardnial Directions
        if (_keyboardInput.IsAction("ui_up"))
        {
            moveToPosition = player.GridPosition + new Vector2(0, -1);
        }
        if (_keyboardInput.IsActionPressed("ui_down"))
        {
            moveToPosition = player.GridPosition + new Vector2(0, 1);
        }
        if (_keyboardInput.IsActionPressed("ui_right"))
        {
            moveToPosition = player.GridPosition + new Vector2(1, 0);
        }
        if (_keyboardInput.IsActionPressed("ui_left"))
        {
            moveToPosition = player.GridPosition + new Vector2(-1, 0);
        }

        // Diagnial Directions
        if (_keyboardInput.IsActionPressed("ui_up") && _keyboardInput.IsActionPressed("ui_right"))
        {
            moveToPosition = player.GridPosition + new Vector2(1, -1);
        }
        if (_keyboardInput.IsActionPressed("ui_up") && _keyboardInput.IsActionPressed("ui_left"))
        {
            moveToPosition = player.GridPosition + new Vector2(-1, -1);
        }
        if (_keyboardInput.IsActionPressed("ui_down") && _keyboardInput.IsActionPressed("ui_right"))
        {
            moveToPosition = player.GridPosition + new Vector2(1, 1);
        }
        if (_keyboardInput.IsActionPressed("ui_left") && _keyboardInput.IsActionPressed("ui_right"))
        {
            moveToPosition = player.GridPosition + new Vector2(-1, 1);
        }

        return moveToPosition;
    }

    private void MovePlayer(Vector2 _moveToPosition)
    {
        _moveToPosition = new Vector2(_moveToPosition.x * 16, _moveToPosition.y * 16);  // Convert from grid position to pixel position
        player.Position = _moveToPosition;
        player.ChangeGridPosition();
        turnManager.EmitSignal("turn_completed");
    }

    private void InteractWithTileOccupant(Vector2 _moveToPosition)
    {
        if (grid.TileGrid[(int)_moveToPosition.x, (int)_moveToPosition.y].Occupant is Enemy)
        {
            Enemy occupant = grid.TileGrid[(int)_moveToPosition.x, (int)_moveToPosition.y].Occupant as Enemy;
            attack.AttackTarget(player, occupant);
        }
        else if (grid.TileGrid[(int)_moveToPosition.x, (int)_moveToPosition.y].Occupant is Door)
        {
            Door occupant = grid.TileGrid[(int)_moveToPosition.x, (int)_moveToPosition.y].Occupant as Door;
            occupant.OpenDoor(player);
        }
        else if (grid.TileGrid[(int)_moveToPosition.x, (int)_moveToPosition.y].Occupant is Chest)
        {
            Chest occupant = grid.TileGrid[(int)_moveToPosition.x, (int)_moveToPosition.y].Occupant as Chest;
            occupant.OpenChest(player);
        }
        // TODO add interaction with doors, chests, items, etc here
    }

    private bool IsPositionWalkable(Vector2 _moveToPosition)
    {
        if (grid.TileGrid[(int)_moveToPosition.x, (int)_moveToPosition.y].SelectedTypeOfTile == Tile.TypeOfTile.Floor)
        {
            return true;
        }
        return false;
    }

    private bool IsPositionOccupied(Vector2 _moveToPosition)
    {
        if (grid.TileGrid[(int)_moveToPosition.x, (int)_moveToPosition.y].IsOccupied)
        {
            return true;
        }
        return false;
    }
}
