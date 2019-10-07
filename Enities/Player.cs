using Godot;
using System;
using System.Collections.Generic;

public class Player : Node2D
{
    Vector2 gridPosition = new Vector2();
    List<Vector2> pathToFollow = new List<Vector2>();

    Stats stats;
    TurnManager turnManager;
    Grid grid;
    PlayerInput playerInput;
    MovementCursor movementCursor;
    Inventory inventory;
    EquipmentHolder equipment;

    public Vector2 GridPosition
    {
        get { return gridPosition; }
    }

    public Stats Stats
    {
        get { return stats; }
    }

    public Inventory Inventory
    {
        get { return inventory; }
    }

    public EquipmentHolder Equipment
    {
        get { return equipment; }
    }

    public override void _Ready()
    {
        grid = GetTree().GetRoot().GetNode("Game/Grid") as Grid;
        turnManager = GetNode("/root/TurnManager") as TurnManager;
        movementCursor = GetNode("MovementCursor") as MovementCursor;
        playerInput = GetNode("PlayerInput") as PlayerInput;
        stats = GetNode("Stats") as Stats;
        inventory = GetNode("Inventory") as Inventory;
        equipment = GetNode("Equipment") as EquipmentHolder;

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

    public void CheckIfAlive()
    {
        if (stats.CurrentHealth <= 0)
        {
            // TODO run the gameover script from here
            GD.Print("THE PLAYER IS DEEEEAAAD!!");
        }
    }
}
