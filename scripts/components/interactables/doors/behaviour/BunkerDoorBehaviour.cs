using BP.GameConsole;
using Godot;

public partial class BunkerDoorBehaviour : DoorBehaviour
{
    [Export] private AnimationTree _animationTree;
    private PlayerController _playerController;
    private Node3D _entryPos;
    private bool _isInteract;
    
    public override void Interact()
    {
        //if(_playerController == null) _playerController = GetTree().GetFirstNodeInGroup("LocalPlayer").GetComponent<PlayerController>();
        //if(_entryPos == null) _entryPos = GetTree().GetFirstNodeInGroup("EnterPoint") as Node3D;
        //_playerController.TeleportTo(_entryPos.GlobalPosition);
    }

    public override void Animate()
    {
        _isInteract = !_isInteract;
        _animationTree.Set("parameters/conditions/isInteract", _isInteract);
        _animationTree.Set("parameters/conditions/isIdle", !_isInteract);
    }
}