using Godot;
using System;

public class Game : Node2D
{
    Player player;
    Grid grid;
    TurnManager turnManager;
    Random random = new Random();

    [Export] PackedScene scorpionScene;
    [Export] PackedScene doorScene;
    [Export] PackedScene chestScene;
    [Export] PackedScene silverKeyScene;

    public Random Random
    {
        get { return random; }
    }

    public override void _Ready()
    {
        grid = GetNode("Grid") as Grid;
        turnManager = GetNode("/root/TurnManager") as TurnManager;
        //SetUpGame();
    }

    public void SetUpGame()  // Temp method!
    {
        GD.Print(grid.TileGrid.Length);
        SetUpGrid();
        
        //AddEnemies(2);
        //AddDoorsAndChests();

        foreach (Tile tile in grid.TileGrid)
        {
            if (tile.SelectedTypeOfTile == Tile.TypeOfTile.Floor && tile.RoomNumber == 0)
            {
                Player player = GetNode("Player") as Player;
                player.Position = new Vector2(tile.GridPosition.x * 16, tile.GridPosition.y * 16);
                break;
            }
        }

        turnManager.RunTurns();
    }

    private void SetUpGrid()
    {
        grid.DungeonGenerator.GenerateDungeon();
        grid.SetTileMap();
        grid.SetDoors();
        grid.SetChests();
        AddEnemies(10);
        //grid.AddTilesAsChildren();
    }

    private void AddEnemies(int _amount)
    {
        for (int i = 0; i < _amount; ++i)
        {
            Enemy scorpion = scorpionScene.Instance() as Enemy;
            AddChild(scorpion);
        }
    }

    private void AddDoorsAndChests()
    {
        Item silverKey = silverKeyScene.Instance() as Item;
        Door door = doorScene.Instance() as Door;
        //door.InitializeDoor(grid, Door.LockState.Locked);
        door.KeyRequired = silverKey as Key;
        AddChild(door);
        Chest chest = chestScene.Instance() as Chest;
        //chest.InitializeChest(grid, silverKey);
        AddChild(chest);
    }
}
