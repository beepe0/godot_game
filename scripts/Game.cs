using Godot;

public partial class Game : Node
{
    public static Game Instance { get; private set; }
    public override void _EnterTree()
    {
        Instance = this;
    }
}