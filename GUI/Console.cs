using Godot;
using System;
using System.Collections.Generic;

public class Console : NinePatchRect
{
    VBoxContainer textContainer;
    List<string> messages = new List<string>();
    int maxMessageAmount = 20;
    [Export] PackedScene messageScene;

    public override void _Ready()
    {
        textContainer = GetNode("ScrollContainer/TextContainer") as VBoxContainer;

        for (int i = 0; i < maxMessageAmount; ++i)
        {
            Label textLabel = messageScene.Instance() as Label;
            textContainer.AddChild(textLabel);
        }
    }

    public void PrintMessageToConsole(string _message)
    {
        _message = "- " + _message;
        messages.Insert(0, _message);

        if (messages.Count > maxMessageAmount)
        {
            messages.Remove(messages[20]);
        }

        Godot.Collections.Array messageChildren = textContainer.GetChildren();
        int currentMessage = 0;

        foreach (Label child in messageChildren)
        {
            child.Text = messages[currentMessage];

            if (messages.Count - 1 > currentMessage)
            {
                currentMessage++;
            }
            else
            {
                break;
            }
        }
    }

    private void ClearMessages()
    {
        Godot.Collections.Array messageChildren = textContainer.GetChildren();

        foreach (Label child in messageChildren)
        {
            child.QueueFree();
        }
    }
}
