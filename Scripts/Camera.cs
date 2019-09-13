using Godot;
using System;

public class Camera : Camera2D
{
    Vector2 mousePosition = new Vector2();
    Vector2 screenSize;
    int moveSpeed = 200;
    int screenEdge = 25;

    TurnManager turnManager;

    public override void _Ready()
    {
        turnManager = GetNode("/root/TurnManager") as TurnManager;
        screenSize = new Vector2(640, 360);

        turnManager.Connect("turn_completed", this, nameof(CenterCameraOnTurnComplete));
    }

    public override void _Process(float delta)
    {
        MoveCamera(delta);
    }

    private void MoveCamera(float delta)
    {
        mousePosition = GetViewport().GetMousePosition();

        if(mousePosition.y <= screenEdge)  // Move up
        {
            Position += new Vector2(0, -moveSpeed * delta);
        }
        if (mousePosition.x <= screenEdge)  // Move left
        {
            Position += new Vector2(-moveSpeed * delta, 0);
        }
        if (mousePosition.x >= screenSize.x - screenEdge)  // Move right
        {
            Position += new Vector2(moveSpeed * delta, 0);
        }
        if (mousePosition.y >= screenSize.y - screenEdge) // Move down
        {
            Position += new Vector2(0, moveSpeed * delta);
        }
    }

    public void CenterCameraOnTurnComplete()
    {
        // Called from turn manager signal. So when player completes the turn this function will be called.
        // Centers camera with player at end of turn

        Position = new Vector2(0,0);
    }
}
