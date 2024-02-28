using BP.GameConsole;
using Godot;
using System;

[GlobalClass]
public partial class Interactable : Node
{
    [Signal]
    public delegate void OnInteractedEventHandler(Node who);
    public virtual void Interact(Node who)
    {
        EmitSignal(SignalName.OnInteracted, who);
    }
}
