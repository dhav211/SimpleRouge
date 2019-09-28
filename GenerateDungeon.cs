using Godot;
using System;

// TEMP SCRIPT!!!


public class GenerateDungeon : Button
{
    Game game;
    
    public override void _Ready()
    {
        game = GetTree().GetRoot().GetNode("Game") as Game;
    }

    public void _on_GenerateDungeon_pressed()
    {
        game.SetUpGame();
        Visible = false;
    }
}
