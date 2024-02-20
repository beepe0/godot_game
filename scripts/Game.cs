using Godot;

public partial class Game : Node
{
    public static Game Instance { get; private set; }

    public RandomNumberGenerator Rng = new RandomNumberGenerator();

    public override void _EnterTree()
    {
        Instance = this;
    }
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
}