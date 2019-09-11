using Godot;
using System;

public class MovementCursor : Sprite
{
    TurnManager turnManager;
    Player player;
    Grid grid;

    Vector2 moveToPosition;

    public Vector2 MoveToPosition
    {
        get { return moveToPosition; }
    }

    public override void _Ready()
    {
        turnManager =  GetNode("/root/TurnManager") as TurnManager;
        grid = GetTree().GetRoot().GetNode("Game/Grid") as Grid;
        player = GetParent() as Player;
    }

    public override void _Process(float delta)
    {
        if (turnManager.CurrentTurn == player)
        {
            Visible = true;

            moveToPosition = SetMoveToPosition();
            RotationDegrees = SetCursorRotation(moveToPosition);
            Position = SetCursorPosition(moveToPosition);

            if (moveToPosition == new Vector2(0,0) || !IsPositionWalkable(moveToPosition))  // Cursor can become invisible during player's turn
            {
                Visible = false;
            }
        }
        else
        {
            Visible = false;
        }
    }

    private Vector2 SetMoveToPosition()
    {
        Vector2 mousePosition = GetGlobalMousePosition();
            mousePosition = new Vector2(Mathf.FloorToInt(mousePosition.x / 16), Mathf.FloorToInt(mousePosition.y / 16));
            Vector2 playerPosition = player.GridPosition;
            Vector2 positionDifference = mousePosition - playerPosition;
            
            return GetReadablePositionDifference(positionDifference);

    }

    private Vector2 GetReadablePositionDifference(Vector2 _positionDifference)
    {
        // Changes the vector into a value more readable and managable such as 1,1 instead of 16,3

        int x = 0;
        int y = 0;

        if (_positionDifference.x > 1)
        {

            x = (int)_positionDifference.x - ((int)_positionDifference.x - 1);
        }
        else if (_positionDifference.x < -1)
        {

            x = (int)_positionDifference.x - ((int)_positionDifference.x + 1);
        }
        else if (_positionDifference.x <= 1 && _positionDifference.x >= -1)
        {
            x = (int)_positionDifference.x;
        }

        if (_positionDifference.y > 1)
        {

            y = (int)_positionDifference.y - ((int)_positionDifference.y - 1);
        }
        else if (_positionDifference.y < -1)
        {

            y = (int)_positionDifference.y - ((int)_positionDifference.y + 1);
        }
        else if (_positionDifference.y <= 1 && _positionDifference.y >= -1)
        {
            y = (int)_positionDifference.y;
        }

        return new Vector2(x,y);
    }

    private float SetCursorRotation(Vector2 _positionDifference)
    {
        // Returns the degree value to tilt the cursor based upon the movement vector

        if (_positionDifference == new Vector2(-1,-1))  // Top Left
        {
            return -45;
        }
        else if (_positionDifference == new Vector2(0,-1))  // Top
        {
            return 0;
        }
        else if (_positionDifference == new Vector2(1,-1))  // Top Right
        {
            return 45;
        }
        else if (_positionDifference == new Vector2(1,0))  // Right
        {
            return 90;
        }
        else if (_positionDifference == new Vector2(1,1))  // Bottom Right
        {
            return 135;
        }
        else if (_positionDifference == new Vector2(0,1))  // Bottom
        {
            return 180;
        }
        else if (_positionDifference == new Vector2(-1,1))  // Bottom Left
        {
            return -135;
        }
        else if (_positionDifference == new Vector2(-1,0))  // Left
        {
            return -90;
        }

        return 0;
    }

    private Vector2 SetCursorPosition(Vector2 _positionDifference)
    {
        return new Vector2(_positionDifference.x * 16, _positionDifference.y * 16);
    }

    private bool IsPositionWalkable(Vector2 _positionDifference)
    {
        // Checks in the tile grid to see if the tile the player will move to is a floor tile and it's empty

        Vector2 positionToMove = player.GridPosition + _positionDifference;

        if (grid.TileGrid[(int)positionToMove.x, (int)positionToMove.y].SelectedTypeOfTile == Tile.TypeOfTile.Floor &&
            !grid.TileGrid[(int)positionToMove.x, (int)positionToMove.y].IsOccupied)
        {
            return true;
        }
        return false;
    }
}
