using Godot;
using System;

public class Game : Node2D
{
    Grid grid;
    TurnManager turnManager;
    Random random = new Random();
    [Export] PackedScene scorpionScene;

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
        
        for (int i = 0; i < 2; ++i)
        {
            Enemy scorpion = scorpionScene.Instance() as Enemy;
            AddChild(scorpion);
        }

        turnManager.RunTurns();
    }

    private void SetUpGrid()
    {
        grid.DungeonGenerator.InitializeDugeonGenerator(grid);
        grid.DungeonGenerator.GenerateDungeon();
        grid.AddTilesAsChildren();
    }
}
