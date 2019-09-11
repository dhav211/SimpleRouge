using Godot;
using System;

public class Cursor : Sprite
{
    Vector2 gridPosition = new Vector2();
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        Vector2 mousePosition = GetGlobalMousePosition();
        gridPosition = new Vector2(Mathf.FloorToInt(mousePosition.x / 16), Mathf.FloorToInt(mousePosition.y / 16));
        Position = new Vector2(gridPosition.x * 16, gridPosition.y * 16);
    }
}
