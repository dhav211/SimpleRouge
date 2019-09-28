using Godot;
using System;

/*
    This script will create a new scene, and also a node for the the sprite.
    It is done this way because godot won't let me add children of packedscenes a to a scene, which i'm not sure if is a bug or a feature.
 */

public class Tile
{
    public Tile(Vector2 _gridPosition)
    {
        GridPosition = _gridPosition;
    }

    public enum TypeOfTile { Empty, Floor, Wall }
    public TypeOfTile SelectedTypeOfTile { get; set; }

    public Vector2 GridPosition { get; set;}

    public int RoomNumber { get; set; }

    public bool IsOccupied { get; set; }  // For walkable tiles, if enemy/player/chest/etc is in this spot, this bool will be true
    public bool IsExit { get; set; }
    public bool IsHall { get; set; }
    public Node2D Occupant { get; set; }

} 
