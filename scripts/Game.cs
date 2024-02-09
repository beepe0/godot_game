using Godot;

public partial class Game : Node
{
    public static Game Instance { get; private set; }
    public static MultiplayerManager MultiplayerManager { get; set; }
    public static LocalPlayer LocalPlayer { get; set; }

    public override void _EnterTree()
    {
        Instance = this;
    }
}