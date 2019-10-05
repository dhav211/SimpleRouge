using Godot;
using System;
using System.Collections.Generic;

public class Console : NinePatchRect
{
    VBoxContainer textContainer;
    ScrollContainer scrollContainer;
    List<string> messages = new List<string>();
    int maxMessageAmount = 20;
    [Export] PackedScene messageScene;

    public override void _Ready()
    {
        textContainer = GetNode("ScrollContainer/TextContainer") as VBoxContainer;
        scrollContainer = GetNode("ScrollContainer") as ScrollContainer;
    }

    public void PrintMessageToConsole(string _message)
    {
        scrollContainer.ScrollVertical = 0;  // Sets the scroll container back to the top when new message is added
        ClearMessages();  // Clears all children when new message comes in
        _message = "- " + _message;  // Adds a dash for easier reading in console
        messages.Insert(0, _message);  // Adds the new message at top of list and pushes all the other ones back

        if (messages.Count > maxMessageAmount)  // Remove any messages that will be over the max limit
        {
            messages.Remove(messages[20]);
        }

        for (int i = 0; i < messages.Count; ++i)  // Finally spawn all messages in order when a new message comes in
        {
            Label messageLabel = messageScene.Instance() as Label;
            messageLabel.Text = messages[i];
            textContainer.AddChild(messageLabel);
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
