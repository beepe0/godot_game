using BP.ComponentSystem;
using BP.GameConsole;
using Godot;

[GlobalClass]
public partial class DoorController : ComponentObject
{
    [Export] private DoorBehaviour _currentBehaviour;
    
    private void _OnInteract(Node node)
    {
        GameConsole.Instance.Debug($"Interact: {node.Name}");
        _currentBehaviour.Interact();
    }
}