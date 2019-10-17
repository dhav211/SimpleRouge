using Godot;
using System;

public class PlayerInfo : NinePatchRect
{
    Label playerName;
    Label playerLVL;
    Label playerCurrentHP;
    Label playerMaxHP;
    Label playerSTR;
    Label playerDEF;
    Player player;

    public override void _Ready()
    {
        playerName = GetNode("PlayerName") as Label;
        playerLVL = GetNode("LVLContainer/CurrentLVL") as Label;
        playerCurrentHP = GetNode("HPContainer/CurrentHP") as Label;
        playerMaxHP = GetNode("HPContainer/MaxHP") as Label;
        playerSTR = GetNode("STRContainer/CurrentSTR") as Label;
        playerDEF = GetNode("DEFContainer/CurrentDEF") as Label;

        //player = GetTree().GetRoot().GetNode("Game/Player") as Player;
        AddToGroup("PlayerInfo");
    }

    public  void FillLabels(Player _player)
    // Done at start of game, fills out every label with what is found in player stats.
    {
        player = _player;

        playerName.Text = player.Stats.EntityName;
        playerLVL.Text = player.Stats.Level.ToString();
        playerCurrentHP.Text = player.Stats.CurrentHealth.ToString();
        playerMaxHP.Text = player.Stats.Health.ToString();
        playerSTR.Text = player.Stats.Strength.ToString();
        playerDEF.Text = player.Stats.Defense.ToString();
    }

    public void UpdatePlayerCurrentHP()
    {
        playerCurrentHP.Text = player.Stats.CurrentHealth.ToString();
    }

    public void UpdatePlayerStrength()
    {
        playerSTR.Text = player.Stats.Strength.ToString();
    }

    public void UpdatePlayerDefense()
    {
        playerDEF.Text = player.Stats.Defense.ToString();
    }
}
