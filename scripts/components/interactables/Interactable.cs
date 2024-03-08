using BP.ComponentSystem;
using BP.GameConsole;
using Godot;
using System;

[GlobalClass]
public partial class Interactable : ComponentObject
{
    [Export] public bool CanInteract = true;

    [Signal]
    public delegate void OnInteractedEventHandler(Node who);
    public virtual void Interact(Node who)
    {
        if (!CanInteract) return;
        EmitSignal(SignalName.OnInteracted, who);
    }
}
