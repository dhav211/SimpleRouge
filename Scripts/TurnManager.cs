using Godot;
using System;
using System.Collections.Generic;

public class TurnManager : Node
{
    List<Node2D> turns = new List<Node2D>();
    Node2D currentTurn;
    bool isTurnManagerRunning = true;
    bool isTurnComplete = false;

    [Signal] delegate void turn_completed();

    Timer timer;

    public List<Node2D> Turns
    {
        get { return turns; }
        set { turns = value; }
    }

    public Node2D CurrentTurn
    {
        get { return currentTurn; }
    }

    public bool IsTurnManagerRunning
    {
        get { return isTurnManagerRunning; }
        set { isTurnManagerRunning = value; }
    }

    public override void _Ready()
    {
        Connect(nameof(turn_completed), this, nameof(_on_Turn_Completed));
    }

    public async void RunTurns()
    {
        Player player = null;
        Enemy enemy = null;
        timer = GetTree().GetRoot().GetNode("Game/TurnTimer") as Timer;

        while (isTurnManagerRunning)
        {
            for (int i = 0; i < turns.Count; i++)
            {
                currentTurn = turns[i];
                if (turns[i] is Player)
                {

                    if (player == null)
                        player = turns[i] as Player;
                    
                    player.StartTurn();
                    await ToSignal(this, "turn_completed");
                    timer.Start();
                    await ToSignal(timer, "timeout");
                    
                }
                else if (turns[i] is Enemy)
                {
                    enemy = turns[i] as Enemy;

                    if (!enemy.IsAlive)
                        continue;

                    enemy.RunAI();

                    // Set the timer to be near instant if enemy is off screen, but on screen the timer will be of normal length
                    Vector2 distance = enemy.Position - player.Position;
                    if (distance.x > 500 || distance.y > 500)
                    {
                        timer.WaitTime = 0.01f;
                    }
                    else
                    {
                        timer.WaitTime = 0.1f;
                    }

                    timer.Start();
                    enemy = null;
                    await ToSignal(timer, "timeout");
                }
            }
        }
    }

    public void TurnCompleted()
    {
        EmitSignal(nameof(turn_completed));
    }

    public void _on_Turn_Completed()
    {
    }
}
