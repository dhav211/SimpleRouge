using Godot;
using System;

/*
    This script will create a new scene, and also a node for the the sprite.
    It is done this way because godot won't let me add children of packedscenes a to a scene, which i'm not sure if is a bug or a feature.
 */

public class Tile : Node2D
{
    Texture texture = ResourceLoader.Load("res://tilesheet.png") as Texture;
    Rect2[] textureLocations = new Rect2[3];

    public enum TypeOfTile { Empty, Floor, Wall }
    public TypeOfTile SelectedTypeOfTile { get; set; }

    public Vector2 GridPosition { get; set;}

    public bool IsOccupied { get; set; }  // For walkable tiles, if enemy/player/chest/etc is in this spot, this bool will be true
    public Node2D Occupant { get; set; }

    Sprite sprite;

    public void CreateTile(TypeOfTile _typeOfTile, int _x, int _y)
    {
        SelectedTypeOfTile = _typeOfTile;
        GridPosition = new Vector2(_x, _y);
        Position = new Vector2(GridPosition.x * 16, GridPosition.y * 16);
        sprite = new Sprite();
        AddChild(sprite);
        sprite.Texture = texture;
        sprite.RegionEnabled = true;
        SetTextureLocations();
        SetTileSprite();
    }

    public void SetTileSprite()
    {
    // Sets the tile based upon the value of the enum given as a parameter.

        switch(SelectedTypeOfTile)
        {
            case Tile.TypeOfTile.Empty:
                sprite.RegionRect = textureLocations[(int)Tile.TypeOfTile.Empty];
                break;
            case Tile.TypeOfTile.Floor:
                sprite.RegionRect = textureLocations[(int)Tile.TypeOfTile.Floor];
                break;
            case Tile.TypeOfTile.Wall:
                sprite.RegionRect = textureLocations[(int)Tile.TypeOfTile.Wall];
                break;
        }
    }

    private void SetTextureLocations()
    {
    // Sets the texture locations array with rects with which sprite should be drawn.

        textureLocations[(int)Tile.TypeOfTile.Empty] = new Rect2(0, 0, 16, 16);
        textureLocations[(int)Tile.TypeOfTile.Floor] = new Rect2(0, 0, 16, 16);
        textureLocations[(int)Tile.TypeOfTile.Wall] = new Rect2(0, 221, 16, 16);
    }
}
