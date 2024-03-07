using Godot;

public partial class DoorBehaviour : Node
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