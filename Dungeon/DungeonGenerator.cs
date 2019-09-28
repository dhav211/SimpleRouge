using Godot;
using System;
using System.Collections.Generic;

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
        List<Room> rooms = new List<Room>();
        int roomsToCreate = 8;
        int roomsCreated = 0;
        // Dictionary has 2 vector2s, first is position of exit, and second is direction from the exit the room will be created
        Dictionary<Vector2,Vector2> exitsToConnect = new Dictionary<Vector2, Vector2>();

        // The main loop for the dungeon generation. Still not sure if I want it to be a void, or simply return a 2D array of tiles
        FillGridWithEmptyTiles();
        CreateInitialRoom(exitsToConnect, roomsCreated, rooms);
        
        while (roomsCreated <= roomsToCreate || exitsToConnect.Count > 0)
        {
            Vector2 exitToConnect = GetFirstKeyOfDictionary(exitsToConnect);
            CreateRoom(exitToConnect, exitsToConnect[exitToConnect], roomsCreated, roomsToCreate, exitsToConnect, rooms); 
            exitsToConnect.Remove(exitToConnect);
            roomsCreated++;
        }

        SetWalls();
        //CreatePillars();
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
                if (grid.TileGrid[x,y] == null)
                {
                    grid.TileGrid[x,y] = new Tile(new Vector2(x,y));
                }
                SetTileAsEmpty(x,y);
            }
        }
    }

    private void CreateInitialRoom(Dictionary<Vector2,Vector2> _exitsToConnect, int _roomsCreated, List<Room> _rooms)
    {

        Vector2 startingPosition = new Vector2();
        Vector2 topLeftCorner = new Vector2();
        List<Vector2> directionsChosen = new List<Vector2>();
        bool isPositionFound = false;
        int roomWidth = GetRandomEvenNumber();
        int roomHeight = GetRandomEvenNumber();

        while (!isPositionFound)
        {
            startingPosition = new Vector2(random.Next(0, grid.GridWidth), random.Next(0, grid.GridHeight));
            topLeftCorner = new Vector2(startingPosition.x - (roomWidth / 2), startingPosition.y - (roomHeight / 2));

            if (IsNewRoomInGridBounds(topLeftCorner, roomWidth, roomWidth))
                isPositionFound = true;
        }

        Room room = new Room(_roomsCreated, startingPosition);
        _rooms.Add(room);
        //topLeftCorner = new Vector2(startingPosition.x - (roomWidth / 2), startingPosition.y - (roomHeight / 2));
        FillRoom(topLeftCorner, roomWidth, roomHeight, _roomsCreated, room);

        while (_exitsToConnect.Count == 0)
            CreateExits(directionsChosen, startingPosition, _exitsToConnect, _roomsCreated);
    }

    private void CreateRoom(Vector2 _exitPosition, Vector2 _directionToHead, int _roomsCreated, int _roomsToCreate, Dictionary<Vector2,Vector2> _exitsToConnect, List<Room> _rooms)
    {
        Vector2 startingPosition = new Vector2();  // Will be the center of the room
        Vector2 topLeftCorner = new Vector2();
        List<Vector2> directionsChosen = new List<Vector2>();
        int roomWidth = GetRandomEvenNumber();
        int roomHeight = GetRandomEvenNumber();
        int minRoomSize = 6;
        int connectingRoomNumber = grid.TileGrid[(int)_exitPosition.x, (int)_exitPosition.y].RoomNumber;
        Room connectingRoom;
        if (connectingRoomNumber < _rooms.Count && connectingRoomNumber >= 0)
        {
            connectingRoom = _rooms[connectingRoomNumber];
        }
        else
        {
            connectingRoom = null;
        }

        bool isCreatingRoom = true;

        while (isCreatingRoom)
        {
            int roomWidthRadius = roomWidth / 2;
            int roomHeightRadius = roomHeight / 2;
            int hallwayDistanceX = (int)_directionToHead.x * 3;
            int hallwayDistanceY = (int)_directionToHead.y * 3;

            startingPosition = new Vector2(_exitPosition.x - (roomWidthRadius * _directionToHead.x + hallwayDistanceX), _exitPosition.y - (roomHeightRadius * _directionToHead.y + hallwayDistanceY));
            topLeftCorner = new Vector2(startingPosition.x - (roomWidth / 2), startingPosition.y - (roomHeight / 2));

            // Check if new room is in bounds
            if (IsNewRoomInGridBounds(topLeftCorner, roomWidth, roomHeight))
            {   
                if (WillRoomOverlap(topLeftCorner, roomWidth, roomHeight))
                {
                    if (roomWidth < minRoomSize || roomHeight < minRoomSize)
                    {
                        isCreatingRoom = false;
                        List<Vector2> overlappingTiles = GetOverlappingTiles(topLeftCorner, roomWidth, roomHeight);

                        if (overlappingTiles.Count > 0)
                        {
                            CreateConnectingHallwayToOverlappingRoom(_exitPosition, overlappingTiles, _roomsCreated);
                        }
                        isCreatingRoom = false;
                    }
                    else
                    {
                        // Shrink the room size and try the process again if it overlaps with another room but not too small to give up
                        roomWidth -= 2;
                        roomHeight -= 2;
                    }
                }
                else
                {
                    Room room = new Room(_roomsCreated, startingPosition);
                    _rooms.Add(room);

                    FillRoom(topLeftCorner, roomWidth, roomHeight, _roomsCreated, room);

                    if (_roomsCreated <= _roomsToCreate)
                    {
                        CreateExits(directionsChosen, startingPosition, _exitsToConnect, _roomsCreated);
                    }
                    _roomsCreated++;
                    isCreatingRoom = false;
                }
            }
            else  // new room won't be in grid bounds, so shrink room and try again
            {
                if (roomWidth < minRoomSize || roomHeight < minRoomSize)
                {
                    isCreatingRoom = false;
                    RemoveHallway(_exitPosition, _directionToHead);
                }
                else
                {
                    roomWidth -= 2;
                    roomHeight -= 2;
                }
            }
        }
    }

    private void RemoveHallway(Vector2 _position, Vector2 _directionToHead)
    {
        // Set a current tile as the one at exit positoin
        // in a while loop, check if current tile is an hall, if it is remove it
        // set the next current tile but adding the position with the direction to head vector
        // repeat loop until current tile isn't a hall

        Tile currentTile = grid.TileGrid[(int)_position.x, (int)_position.y];

        while (currentTile.IsHall)
        {
            currentTile.SelectedTypeOfTile = Tile.TypeOfTile.Empty;
            currentTile.IsExit = false;
            currentTile.IsHall = false;

            _position -= _directionToHead;
            if (IsTileInGridBounds(_position))
                currentTile = grid.TileGrid[(int)_position.x, (int)_position.y];
        }
    }

    private void CreateExits(List<Vector2> _directionsChosen, Vector2 _startingPosition, Dictionary<Vector2,Vector2> _exitsToConnect, int _roomNumber)
    {
        int numberOfExits = random.Next(1,5);
        int hallwayLength = 4;

        for (int i = 0; i < numberOfExits; ++i)
        {
            bool isDirectionChosen = false;
            Vector2 directionToHead = new Vector2();
            while (!isDirectionChosen)
            {
                // TODO there needs to be a check to make sure it's not making an exit to a room it's already connected to
                directionToHead = GetDirectionToHead(random.Next(0,4));  // Assigns which side of room exit will be on

                if (HasDirectionForExitBeenChosen(_directionsChosen, directionToHead))
                    continue;  // if this side of the room has been chosen already, then try again.
                _directionsChosen.Add(directionToHead);
                isDirectionChosen = true;
            }
            // find the nearest empty tile in that direction
            // set that empty tile as a floor and an exit

            bool isExitFound = false;
            int distanceToCheck = 1;
            int XDistance = 0;
            int YDistance = 0;

            while (!isExitFound)
            {
                // TODO lets not use starting position, choose a random spot in the room and work with that.

                // Get the tile to check by multiplying the current distance with the direction vector, this will give a positve or negative number or 0
                XDistance = (int)_startingPosition.x - (distanceToCheck * (int)directionToHead.x);
                YDistance = (int)_startingPosition.y - (distanceToCheck * (int)directionToHead.y);
                if (IsTileInGridBounds(new Vector2(XDistance, YDistance)))
                {
                    if (grid.TileGrid[XDistance, YDistance].SelectedTypeOfTile == Tile.TypeOfTile.Empty)
                    {
                        // Hall found, so set the tile as a floor and then make it an exit.
                        _exitsToConnect.Add(new Vector2(XDistance, YDistance), directionToHead);

                        Vector2[] hallwayPositions = new Vector2[hallwayLength];

                        for (int j = 0; j < hallwayLength; j++)  // Sets the positions of the exit and hallway.
                        {
                            XDistance = (int)_startingPosition.x - (distanceToCheck * (int)directionToHead.x);
                            YDistance = (int)_startingPosition.y - (distanceToCheck * (int)directionToHead.y);

                            if (IsTileInGridBounds(new Vector2(XDistance, YDistance)))
                            {
                                hallwayPositions[j] = new Vector2(XDistance, YDistance);
                                distanceToCheck++;
                            }
                            else
                            {
                                break;  // tile is out of bounds, cancel the exit building.
                            }
                        }
                        isExitFound = true;

                        if (isExitFound)  // It is possible to build an exit here, so another for loop will be ran to actually set the hall
                        {
                            for (int j = 0; j < hallwayLength; ++j)
                            {
                                SetTileAsFloor((int)hallwayPositions[j].x, (int)hallwayPositions[j].y, _roomNumber);

                                if (j == 0) // the first tile of each hall will be considered the exit, doors will be placed here
                                    grid.TileGrid[(int)hallwayPositions[j].x, (int)hallwayPositions[j].y].IsExit = true;
                                grid.TileGrid[(int)hallwayPositions[j].x, (int)hallwayPositions[j].y].IsHall = true;
                            }
                        }
                    }
                    else if (grid.TileGrid[XDistance, YDistance].SelectedTypeOfTile == Tile.TypeOfTile.Floor)
                    {
                        // An empty tile wasn't found, so keep checking further out.
                        distanceToCheck++;
                        continue;
                    }
                }
                else // Since exit is outside of bounds, cancel the exit
                {
                    isExitFound = true;
                }
            }
        }
    }

    private void FillRoom(Vector2 _startingPosition, int _roomWidth, int _roomHeight, int _roomNumber, Room _room)
    // Dual purpose, creates the tiles of the room, and then adds the floor tile positions to the room
    {
        for (int x = 0; x < _roomWidth; x++)
        {
            for (int y = 0; y < _roomHeight; y++)
            {
                SetTileAsFloor((int)_startingPosition.x + x, (int)_startingPosition.y + y, _roomNumber);
                _room.tilesInRoom.Add(new Vector2(_startingPosition.x + x, _startingPosition.y + y));
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

    private void SetEmptySurroundTilesAsWalls(Vector2 _gridPosition)
    {
        // Checks every surround tile around tile to see if it's empty, if it is, then it should be a wall.

        Vector2[] surroundingTiles = GetSurroundingTilesPosition(_gridPosition);
        for (int i = 0; i < surroundingTiles.Length; ++i)
        {
            if (IsTileInGridBounds(new Vector2(surroundingTiles[i].x, surroundingTiles[i].y)) && 
                grid.TileGrid[(int)surroundingTiles[i].x, (int)surroundingTiles[i].y].SelectedTypeOfTile == Tile.TypeOfTile.Empty)

                SetTileAsWall((int)surroundingTiles[i].x, (int)surroundingTiles[i].y);

            else if (!IsTileInGridBounds(new Vector2(surroundingTiles[i].x, surroundingTiles[i].y)))
            {
                SetTileAsWall((int)_gridPosition.x, (int)_gridPosition.y);
            }
        }
    }

    private void CreateConnectingHallwayToOverlappingRoom(Vector2 _exitPosition, List<Vector2> _overlappingTiles, int _roomNumber)
    {
        // If no room can be built because space is too small, then a hallway will be formed.
        // This will look for the closest overlapping tile and then build a hallway to it, in an L shape if need be

        Vector2 closestOverlappingPosition = GetClosestTile(_exitPosition, _overlappingTiles);
        int XDistanceToTravel = (int)closestOverlappingPosition.x - (int)_exitPosition.x;
        int YDistanceToTravel = (int)closestOverlappingPosition.y - (int)_exitPosition.y;
        Vector2 currentPosition = _exitPosition;

        for (int x = 0; x < Math.Abs(XDistanceToTravel + 2); ++x)  // Create hallway horizontally
        {
            if (XDistanceToTravel > 0)
            {
                currentPosition = new Vector2(currentPosition.x + 1, currentPosition.y);
            }
            else if (XDistanceToTravel < 0)
            {
                currentPosition = new Vector2(currentPosition.x - 1, currentPosition.y);
            }
            SetTileAsFloor((int)currentPosition.x, (int)currentPosition.y, _roomNumber);
        }

        for (int y = 0; y < Math.Abs(YDistanceToTravel + 2); ++y)  // Create hallway vertically
        {
            if (YDistanceToTravel > 0)
            {
                currentPosition = new Vector2(currentPosition.x, currentPosition.y + 1);
            }
            else if (YDistanceToTravel < 0)
            {
                currentPosition = new Vector2(currentPosition.x, currentPosition.y - 1);
            }
            SetTileAsFloor((int)currentPosition.x, (int)currentPosition.y, _roomNumber);
        }
    }

    private void SetTileAsFloor(int _x, int _y, int _roomNumber)
    {
        grid.TileGrid[_x, _y].SelectedTypeOfTile = Tile.TypeOfTile.Floor;
        grid.TileGrid[_x, _y].RoomNumber = _roomNumber;
    }

    private void SetTileAsWall(int _x, int _y)
    {
        // Called from SetEmptySurroundTilesAsWalls method, sets given tile in tilegrid as a wall
        grid.TileGrid[_x, _y].SelectedTypeOfTile = Tile.TypeOfTile.Wall;
    }

    private void SetTileAsEmpty(int _x, int _y)
    {
        grid.TileGrid[_x, _y].SelectedTypeOfTile = Tile.TypeOfTile.Empty;
    }

    private int GetRandomEvenNumber()
    {
        int randomNumber = 1;

        do
        {
            randomNumber = random.Next(10,27);
        }
        while (randomNumber % 2 != 0);

        return randomNumber;
    }

    private Vector2 GetClosestTile(Vector2 _startingTile, List<Vector2> _tiles)
    {
        // Requires a list of tiles, finds the closest tile by checking each on and comparing distance from the starting tile.

        Vector2 closestTile = _tiles[0];

        foreach (Vector2 tile in _tiles)
        {
            int closestTileXDistance = (int)closestTile.x - (int)_startingTile.x;
            int closestTileYDistance = (int)closestTile.y - (int)_startingTile.y;
            int closestTileDistance = (closestTileXDistance * closestTileXDistance) + (closestTileYDistance * closestTileYDistance);

            int currentTileXDistance = (int)tile.x - (int)_startingTile.x;
            int currentTileYDistance = (int)tile.y - (int)_startingTile.y;
            int currentTileDistance = (currentTileXDistance * currentTileXDistance) + (currentTileYDistance * currentTileYDistance);

            if (currentTileDistance < closestTileDistance)
            {
                closestTile = tile;
            }
        }
        
        return closestTile;
    }

    private List<Vector2> GetOverlappingTiles(Vector2 _startingPosition, int _roomWidth, int _roomHeight)
    {
        //Does a flood fill of new room, when tiles overlap it adds them to a list.
        // Is used to check for the nearest tile for hall building.

        List<Vector2> overlappingTiles = new List<Vector2>();

        for (int x = 0; x < _roomWidth; ++x)
        {
            for (int y = 0; y < _roomHeight; ++y)
            {
                if (grid.TileGrid[(int)_startingPosition.x + x, (int)_startingPosition.y + y].SelectedTypeOfTile == Tile.TypeOfTile.Floor &&
                    !grid.TileGrid[(int)_startingPosition.x + x, (int)_startingPosition.y + y].IsExit)
                {
                    overlappingTiles.Add(new Vector2(_startingPosition.x + x, _startingPosition.y + y));
                }
            }
        }

        return overlappingTiles;
    }

        private Vector2 GetDirectionToHead(int _i)
    {
        if (_i == 0)
        {
            return new Vector2(0, -1); // Up
        }
        else if (_i == 1)
        {
            return new Vector2(0, 1); // Down
        }
        else if (_i == 2)
        {
            return new Vector2(1, 0); // Right
        }
        else if (_i == 3)
        {
            return new Vector2(-1, 0); // Left
        }

        return new Vector2(0,0); // Error, can't be reached ever.
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

    private Vector2 GetFirstKeyOfDictionary(Dictionary<Vector2,Vector2> _exitsToConnect)
    {
        // Simple helper script to return the first Key in the dictionary.

        foreach (Vector2 key in _exitsToConnect.Keys)
        {
            return key;
        }

        return new Vector2(); // Can't be reached
    }

    private bool WillRoomOverlap(Vector2 _startingPosition, int _roomWidth, int _roomHeight)
    {
        // Checks to see if new room will overlap an already established room.
        _startingPosition = new Vector2(_startingPosition.x - 1, _startingPosition.y - 1);

        for (int x = 0; x < _roomWidth + 2; ++x)
        {
            for (int y = 0; y < _roomHeight + 2; ++y)
            {
                if (grid.TileGrid[(int)_startingPosition.x + x, (int)_startingPosition.y + y].SelectedTypeOfTile == Tile.TypeOfTile.Floor &&
                    !grid.TileGrid[(int)_startingPosition.x + x, (int)_startingPosition.y + y].IsExit &&
                    !grid.TileGrid[(int)_startingPosition.x + x, (int)_startingPosition.y + y].IsHall)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool HasDirectionForExitBeenChosen(List<Vector2> _directionsChosen, Vector2 _directionToHead)
    {
        foreach (Vector2 direction in _directionsChosen)
        {
            if (direction == _directionToHead)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsNewRoomInGridBounds(Vector2 _position, int _widthRadius, int _heightRadius)
    {
        Vector2 alteredPosition = new Vector2(_position.x - 1, _position.y - 1);
        _widthRadius += 2;
        _heightRadius += 2;

        if (alteredPosition.x - _widthRadius < 0)
        {
            return false;
        }
        if (alteredPosition.x + _widthRadius >= grid.GridWidth)
        {
            return false;
        }
        if (alteredPosition.y - _heightRadius < 0)
        {
            return false;
        }
        if (alteredPosition.y + _heightRadius >= grid.GridHeight)
        {
            return false;
        }

        return true;
    }

    private bool IsTileInGridBounds(Vector2 _position)
    {
        if (_position.x < 0)
        {
            return false;
        }
        if (_position.x >= grid.GridWidth)
        {
            return false;
        }
        if (_position.y < 0)
        {
            return false;
        }
        if (_position.y >= grid.GridHeight)
        {
            return false;
        }

        return true;
    }
}

class Room
{
    public Room (int _roomNumber, Vector2 _roomCenter)
    {
        roomNumber = _roomNumber;
        roomCenter = _roomCenter;
    }

    int roomNumber;
    Vector2 roomCenter = new Vector2();
    public List<Vector2> exits = new List<Vector2>();
    public List<Vector2> tilesInRoom = new List<Vector2>();
    public List<Dictionary<int,Vector2>> connectingRooms = new List<Dictionary<int, Vector2>>();  // Any room which is connected to this room
    public List<int> cameFromRooms = new List<int>();  // Rooms which have built a hallway to this room  // TODO think of a better variable name if need be
}
