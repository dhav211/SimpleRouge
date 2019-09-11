using Godot;
using System;

// TODO delete this script when testing is thru

public class Square : Sprite
{
    public override void _Ready()
    {
        Timer timer = new Timer();
        timer.Connect("timeout", this, nameof(_on_Timer_timeout));
        timer.WaitTime = 20;
        AddChild(timer);
        timer.Start();
    }

    public void _on_Timer_timeout()
    {
        QueueFree();
    }
}
