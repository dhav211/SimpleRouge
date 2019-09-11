using Godot;
using System;
using System.Collections.Generic;

public class Player : Node2D
{
    Vector2 gridPosition = new Vector2();
    List<Vector2> pathToFollow = new List<Vector2>();

    Stats stats = new Stats();
    TurnManager turnManager;
    Grid grid;
    PlayerInput playerInput;
    MovementCursor movementCursor;

    public Vector2 GridPosition
    {
        get { return gridPosition; }
    }

    public override void _Ready()
    {
        grid = GetTree().GetRoot().GetNode("Game/Grid") as Grid;
        turnManager = GetNode("/root/TurnManager") as TurnManager;
        movementCursor = GetNode("MovementCursor") as MovementCursor;
        playerInput = GetNode("PlayerInput") as PlayerInput;

        turnManager.Turns.Add(this);
    }

    public void StartTurn()
    {
        ChangeGridPosition();
    }
    
    public void ChangeGridPosition()
    {
        // Changes grid position of player and changes IsOccupied bool in tileGrid.
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].IsOccupied = false;  // sets isOccupied on old grid position on tileGrid to false
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].Occupant = null;

        gridPosition = new Vector2(Mathf.FloorToInt(Position.x /16), Mathf.FloorToInt(Position.y /16));  // Change gridPosition based upon current Position

        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].IsOccupied = true; // sets isOccupied on old grid position on tileGrid to true
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].Occupant = this;
    }
}
