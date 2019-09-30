using Godot;
using System;
using System.Collections.Generic;

public class Grid : TileMap
{
    static int gridHeight = 400;
    static int gridWidth = 400;

    Tile[,] tileGrid = new Tile[gridHeight, gridWidth];
    List<Door> doors = new List<Door>();
    List<Chest> chests = new List<Chest>();

    DungeonGenerator dungeonGenerator;
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

    public List<Door> Doors
    {
        get { return doors; }
        set { doors = value; }
    }

    public List<Chest> Chests
    {
        get { return chests; }
        set { chests = value; }
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
        dungeonGenerator = new DungeonGenerator(this);
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
    
    public void SetDoors()
    {
        foreach (Door door in doors)
        {
            AddChild(door);
        }
    }

    public void SetChests()
    {
        foreach (Chest chest in chests)
        {
            AddChild(chest);
        }
    }
}
