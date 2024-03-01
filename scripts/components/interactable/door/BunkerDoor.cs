using BP.GameConsole;
using BP.ComponentSystem;
using Godot;

namespace TeamGame.scripts.components.interactable.door;

public partial class BunkerDoor : Node
{
    private PlayerController _playerController;
    private Node3D _entryPos;
    
    public void _OnInteract(Node who)
    {
        if(_playerController == null) _playerController = GetTree().GetFirstNodeInGroup("LocalPlayer").GetComponent<PlayerController>();
        if(_entryPos == null) _entryPos = GetTree().GetFirstNodeInGroup("EnterPoint") as Node3D;
        _playerController.TeleportTo(_entryPos.GlobalPosition);
    }
}