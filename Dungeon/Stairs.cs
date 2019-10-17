using Godot;
using System;

public class Stairs : Sprite
{
    public enum StairDirection { Up, Down }
    public StairDirection SelectedDirection { get; set; }

    public Vector2 SpawnLocation { get; set; }
    private Rect2[] spriteRegions = new Rect2[2];
    private Vector2 positionToSet;
    private Vector2 gridPosition;
    private Grid grid;
    private Game game;

    public override void _Ready()
    {
        game = GetTree().GetRoot().GetNode("Game") as Game;
        Position = positionToSet;
        FillSpriteRegions();
        RegionRect = SetSpriteRegion();
    }

    /*
            The dungeon generator will place the stairs, then the direction and finally check for a suitable spawn location, which will be the first non
        occupied space it surrounds.
            The stairs will always be placed in first and last room, first room goes up, last room goes down.
            Except for first and last floor, since first won't have stairs going up and last won't have stairs going down.
            Once the stairs are fully spawned, the ready method will run.
            This will set the sprite for the stairs depending on it's direction.
            when player walks up to the stairs a few things will happen.
            The current floor will change depending on the direction.
            All parts of the grid will be freed, doors, chests, enemies, the grid itself.
            Once that is cleared a new grid will be set.
     */

    public void InitializeStair(Grid _grid, Vector2 _positionToSet, StairDirection _direction)
    {
        grid = _grid;

        positionToSet = _positionToSet;
        gridPosition = new Vector2(Mathf.FloorToInt(positionToSet.x /16), Mathf.FloorToInt(positionToSet.y /16));
        SelectedDirection = _direction;
    }

    public void SetStair()
    {
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].IsOccupied = true;
        grid.TileGrid[(int)gridPosition.x, (int)gridPosition.y].Occupant = this;
    }

    private void FillSpriteRegions()
    {
        spriteRegions[(int)StairDirection.Up] = new Rect2(34, 102, 16, 16);
        spriteRegions[(int)StairDirection.Down] = new Rect2(51, 102, 16, 16);
    }

    private Rect2 SetSpriteRegion()
    {
        if (SelectedDirection == StairDirection.Up)
        {
            return spriteRegions[(int)StairDirection.Up];
        }
        else if (SelectedDirection == StairDirection.Down)
        {
            return spriteRegions[(int)StairDirection.Down];
        }
        else
        {
            GD.PrintErr("Stair direction not set!");
            return new Rect2();
        }
    }

    public void ChangeFloor()
    {
        if (SelectedDirection == StairDirection.Up)
        {
            game.CurrentFloor--;
        }
        if (SelectedDirection == StairDirection.Down)
        {
            game.CurrentFloor--;
        }

        // Do the floor stuff
    }
}
