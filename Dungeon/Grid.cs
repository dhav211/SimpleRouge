using Godot;
using System;

public class Grid : Node2D
{
    static int gridHeight = 100;
    static int gridWidth = 100;

    Tile[,] tileGrid = new Tile[gridHeight, gridWidth];

    DungeonGenerator dungeonGenerator = new DungeonGenerator();
    Random random = new Random();

    public Random Random
    {
        get { return random; }
    }

    public Tile[,] TileGrid 
    {
        get { return tileGrid; }
        set { tileGrid = value; }
    }

    public DungeonGenerator DungeonGenerator
    {
        get { return dungeonGenerator; }
    }

    public int GridHeight
    {
        get { return gridHeight;}
    }

    public int GridWidth
    {
        get { return gridWidth; }
    }

    public override void _Ready()
    {
        
    }

    public void AddTilesAsChildren()
    // Tiles added to tileGrid from dungeon generator will be added as children of the grid
    {
        foreach (Tile tile in tileGrid)
        {
            AddChild(tile);
        }
    }

    private void RemoveTilesAsChildren()
    {
        // TOOD, go thru all children and quene free them
    }
}
