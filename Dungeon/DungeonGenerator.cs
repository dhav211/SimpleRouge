using Godot;
using System;

public class DungeonGenerator
{
    Grid grid;
    Random random;

    public void InitializeDugeonGenerator(Grid _grid)
    {
        // Posibly a dummy method at this point, remove if better solution is reached.
        grid = _grid;
        random = grid.Random;
    }

    public void GenerateDungeon()
    {
        // The main loop for the dungeon generation. Still not sure if I want it to be a void, or simply return a 2D array of tiles
        FillGridWithEmptyTiles();
        CreateSquareRoomInRandomPosition();
        SetWalls();
        CreatePillars();
    }

    private void CreatePillars() // TEMP METHOD
    {
        int numberOfPillars = random.Next(9,30);
        Vector2 spawnLocation = new Vector2();

        for (int i = 0; i < numberOfPillars; ++i)
        {
            spawnLocation = new Vector2(random.Next(1,18), random.Next(1,18));

            for (int x = 0; x < 2; ++x)
            {
                for (int y = 0; y < 2; ++y)
                {
                    grid.TileGrid[(int)spawnLocation.x + x, (int)spawnLocation.y + y].SelectedTypeOfTile = Tile.TypeOfTile.Wall;
                    grid.TileGrid[(int)spawnLocation.x + x, (int)spawnLocation.y + y].SetTileSprite();
                }
            }
        }
    }

    private void FillGridWithEmptyTiles()
    {
        // Goes thru the entire 2D array of tiles, and sets them all as empty tiles.
        for (int x = 0; x < grid.GridWidth; x++)
        {
            for (int y = 0; y < grid.GridHeight; y++)
            {
                CreateEmptyTile(x,y);
            }
        }
    }

    private void CreateEmptyTile(int _x, int _y)
    {
        // Creates an empty tile and places it in the tileGrid of the grid class.
        Tile tile = new Tile();
        tile.CreateTile(tile.SelectedTypeOfTile = Tile.TypeOfTile.Empty, _x, _y);
        grid.TileGrid[_x,_y] = tile;
    }

    private void CreateSquareRoomInRandomPosition()
    {
        // Sets a square set of tiles to floor in a random position of the room.

        // TODO this should actually place it in a random position.
        // TODO once this places in a random position, get a random size it will be, then check that area if it won't over lap an already created room.
        // TODO DON"T USE FOREACH LOOPS, TAKE ADVANTAGE OF 2D ARRAY!!!
        Vector2 startingPosition = new Vector2(1,1);
        Vector2 roomSize = new Vector2(20,20);

        for (int x = 0; x < roomSize.x; x++)
        {
            for (int y = 0; y < roomSize.y; y++)
            {
                grid.TileGrid[(int)startingPosition.x + x, (int)startingPosition.y + y].SelectedTypeOfTile = Tile.TypeOfTile.Floor;
                grid.TileGrid[(int)startingPosition.x + x, (int)startingPosition.y + y].SetTileSprite();
            }
        }
    }

    private void SetWalls()
    {
        // Goes thru every tile in tile, if it's a floor, then it will call the SetEmptySurroundTilesAsWalls method to check if any surround tiles can be a wall
        foreach(Tile tile in grid.TileGrid)
        {
            if (tile.SelectedTypeOfTile == Tile.TypeOfTile.Floor)
            {
                SetEmptySurroundTilesAsWalls(tile.GridPosition);
            }
        }
    }

    private void SetTileAsWall(int _x, int _y)
    {
        // Called from SetEmptySurroundTilesAsWalls method, sets given tile in tilegrid as a wall
        grid.TileGrid[_x, _y].SelectedTypeOfTile = Tile.TypeOfTile.Wall;
        grid.TileGrid[_x, _y].SetTileSprite();
    }

    private void SetEmptySurroundTilesAsWalls(Vector2 _gridPosition)
    {
        // Checks every surround tile around tile to see if it's empty, if it is, then it should be a wall.

        Vector2[] surroundingTiles = GetSurroundingTilesPosition(_gridPosition);
        for (int i = 0; i < surroundingTiles.Length; ++i)
        {
            if (grid.TileGrid[(int)surroundingTiles[i].x, (int)surroundingTiles[i].y].SelectedTypeOfTile == Tile.TypeOfTile.Empty)
                SetTileAsWall((int)surroundingTiles[i].x, (int)surroundingTiles[i].y);
        }
    }

    private Vector2[] GetSurroundingTilesPosition(Vector2 _gridPosition)
    {   
        // Fills and returns an array with the positions of all 8 surrounding tiles
        Vector2 [] surroundingTiles = new Vector2[8];

        surroundingTiles[0] = _gridPosition - new Vector2(-1, -1);
        surroundingTiles[1] = _gridPosition - new Vector2(0, -1);
        surroundingTiles[2] = _gridPosition - new Vector2(1, -1);
        surroundingTiles[3] = _gridPosition - new Vector2(1, 0);
        surroundingTiles[4] = _gridPosition - new Vector2(1, 1);
        surroundingTiles[5] = _gridPosition - new Vector2(0, 1);
        surroundingTiles[6] = _gridPosition - new Vector2(-1, 1);
        surroundingTiles[7] = _gridPosition - new Vector2(-1, 0);

        return surroundingTiles;
    }
}
