using Godot;
using System;

public class Game : Node2D
{
    Player player;
    Grid grid;
    TurnManager turnManager;
    Random random = new Random();

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
        grid = GetNode("Grid") as Grid;
        turnManager = GetNode("/root/TurnManager") as TurnManager;
        SetUpGame();
    }

    public void SetUpGame()  // Temp method!
    {
        SetUpGrid();

        Player player = GetNode("Player") as Player;

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
}
