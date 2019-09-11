using Godot;
using System;

public class TurnTimer : Timer
{
    public override void _Ready()
    {
        
    }

    private void _on_TurnTimer_timeout()
    {
        GD.Print("The timer has finished");
        Stop();
    }
}
