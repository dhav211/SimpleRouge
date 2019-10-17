using Godot;
using System;

public class SideMenu : NinePatchRect
{
    public override void _Ready()
    {
        
    }

    public void SetGUIOnGameStart(Player _player)
    {
        ItemContainer itemContainer = GetNode("Items/ItemContainer") as ItemContainer;
        PlayerInfo playerInfo = GetNode("PlayerInfo") as PlayerInfo;
        EquipMenu equipMenu = GetTree().GetRoot().GetNode("Game/CanvasLayer/GUI/EquipMenu") as EquipMenu;

        playerInfo.FillLabels(_player);
        itemContainer.InitializeItemAmounts(_player);
        equipMenu.SetPlayer(_player);
    }
}
