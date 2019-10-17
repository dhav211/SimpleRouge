using Godot;
using System;
using System.Collections.Generic;

public class Game : Node2D
{
    Player player;
    Grid grid;
    TurnManager turnManager;
    Random random = new Random();
    Grid[] floors = new Grid[5];
    public int CurrentFloor { get; set; }

    [Export] PackedScene gridScene;
    [Export] PackedScene playerScene;
    [Export] PackedScene scorpionScene;
    [Export] PackedScene potionScene;
    [Export] PackedScene weaponScene;
    [Export] PackedScene armorScene;
    [Export] PackedScene secondWeaponScene;

    public Random Random
    {
        get { return random; }
    }

    public override void _Ready()
    {
        turnManager = GetNode("/root/TurnManager") as TurnManager;
        SetUpGame();
    }

    public void SetUpGame()  // Temp method!
    {
        CurrentFloor = 0;

        GenerateDungeon();
        SetUpGrid();

        Player player = playerScene.Instance() as Player;
        AddChild(player);

        foreach (Tile tile in grid.TileGrid)
        {
            if (tile.SelectedTypeOfTile == Tile.TypeOfTile.Floor && tile.RoomNumber == 0)
            {
                player.Position = new Vector2(tile.GridPosition.x * 16, tile.GridPosition.y * 16);
                break;
            }
        }

        ConsumableItem potion = potionScene.Instance() as ConsumableItem;
        player.Inventory.AddItem(potion);

        Equipment weapon = weaponScene.Instance() as Equipment;
        player.Inventory.AddItem(weapon);
        player.Equipment.Equip(weapon);
        Equipment armor = armorScene.Instance() as Equipment;
        player.Inventory.AddItem(armor);
        player.Equipment.Equip(armor);
        Equipment secondWeapon = secondWeaponScene.Instance() as Equipment;
        player.Inventory.AddItem(secondWeapon);

        turnManager.RunTurns();
    }

    private void GenerateDungeon()
    {
        for (int i = 0; i < floors.Length; ++i)
        {
            floors[i] = gridScene.Instance() as Grid;
            floors[i].InitilaizeGrid(random);
            floors[i].DungeonGenerator.GenerateDungeon(i + 1);  // Requires a + 1 because the dungeon generator doesn't start at 0
        }
    }

    public void SetUpGrid()
    {
        grid = floors[CurrentFloor];
        AddChild(grid);
        grid.SetTileMap();
        grid.SetDoors();
        grid.SetChests();
        grid.SetStairs();
        //AddEnemies(10);
        //grid.AddTilesAsChildren();
    }

    public void ClearGrid()
    {
        grid.QueueFree();
    }

    public void SetPlayerFromStairs(Stairs.StairDirection _comingFromDirection)
    {
        if (_comingFromDirection == Stairs.StairDirection.Up)
        {
            // Find the stairs with direction down and spawn player there
            
        }
        else if (_comingFromDirection == Stairs.StairDirection.Down)
        {
            // Find the stairs with direction up and spawn player there
        }
    }

    private void AddEnemies(int _amount)
    {
        for (int i = 0; i < _amount; ++i)
        {
            Enemy scorpion = scorpionScene.Instance() as Enemy;
            AddChild(scorpion);
        }
    }
}
