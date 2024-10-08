using BP.ComponentSystem;
using Godot;

public partial class DoorBehaviour : ComponentObject
{
    public virtual void Interact(){}
    public virtual void Animate(){}
    
    public enum StateDoorBehaviour
    {
        FakeBunkerDoor,
        RoomDoor,
        BunkerDoor
    }
}