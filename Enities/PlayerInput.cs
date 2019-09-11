using Godot;
using System;

public class PlayerInput : Node2D
{
    Vector2 moveToPosition;
    Player player;
    TurnManager turnManager;
    MovementCursor movementCursor;
    Grid grid;

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
                HandleInput(mouseInput);
            }
        }
    }

    private void HandleInput(InputEventMouseButton _mouseInput)
    {
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
                    // TODO this will later battle enemies, open chests, and pick up keys
                }
            }
            else if (!IsPositionWalkable(positionToMove) || movementCursor.MoveToPosition == new Vector2(0,0))  // In valid turn, skip turn
            {
                turnManager.EmitSignal("turn_completed");
            }
        }
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
            GD.Print("Attack " + occupant.Name + "!!!");
        } 
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
