using BP.GameConsole;
using Godot;

public partial class Game : Node
{
    public static Game Instance { get; private set; }

    public override void _EnterTree()
    {
        Instance = this;
    }
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Process(double delta)
    {
        DebugPlayer.Instance.Debug($"Performance.GetMonitor");
        DebugPlayer.Instance.Debug($"TimeFps: {Performance.GetMonitor(Performance.Monitor.TimeFps)}");
        DebugPlayer.Instance.Debug($"RenderTotalDrawCallsInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame)}");
        DebugPlayer.Instance.Debug($"RenderTotalPrimitivesInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalPrimitivesInFrame)}");
        DebugPlayer.Instance.Flush();
    }
}