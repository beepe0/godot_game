using Godot;
using System;
using BP.ComponentSystem;

[GlobalClass]
public partial class DoorAnimator : ComponentObject
{
    [Export] private DoorBehaviour _currentBehaviour;
    
    private void _OnInteract(Node node)
    {
        _currentBehaviour.Animate();
    }
}
