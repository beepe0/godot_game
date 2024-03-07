using Godot;

public partial class RoomDoorBehaviour : DoorBehaviour
{
    [Export] private AnimationTree _animationTree;
    private bool _isInteract;

    public override void Interact()
    {

    }
    public override void Animate()
    {
        _isInteract = !_isInteract;
        _animationTree.Set("parameters/conditions/isInteract", _isInteract);
        _animationTree.Set("parameters/conditions/isIdle", !_isInteract);
    }
}