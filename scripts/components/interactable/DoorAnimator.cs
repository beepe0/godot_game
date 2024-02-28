using BP.GameConsole;
using Godot;
using System;

public partial class DoorAnimator : Node
{
    [Export] private AnimationTree _animationTree;

    private bool _isInteract;

    private void _OnInteract(Node node)
    {
        GameConsole.Instance.Debug($"Interact: {node.Name}");
        _isInteract = !_isInteract;
        _animationTree.Set("parameters/conditions/isOpen", _isInteract);
        _animationTree.Set("parameters/conditions/isClose", !_isInteract);
    }
}
