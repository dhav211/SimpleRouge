using Godot;
using System;

public class Grid : TileMap
{
    static int gridHeight = 200;
    static int gridWidth = 200;

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

    public void SetTileMap()
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            for (int y = 0; y < gridHeight; ++y)
            {
                if (tileGrid[x,y].SelectedTypeOfTile == Tile.TypeOfTile.Floor)
                {
                    SetCell(x, y, 0);
                }
                else if (tileGrid[x,y].SelectedTypeOfTile == Tile.TypeOfTile.Empty)
                {
                    SetCell(x, y, 0);
                }
                else if (tileGrid[x,y].SelectedTypeOfTile == Tile.TypeOfTile.Wall)
                {
                    SetCell(x, y, 1);
                }
            }
        }
    }
}
