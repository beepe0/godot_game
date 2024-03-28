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
		DebugDraw.Text((ulong)(1.2 * 1000.0f), 0, Colors.Aqua, 1 << 1);
		//DebugDraw.Text3D("Test", new Vector3(100, 0, 0), 1, Colors.Azure);
		// DebugDraw.TextKeyed("test", "test11111", 1, Colors.Bisque);
		// DebugDraw.TextKeyed("2test", "asd", 1, Colors.Bisque);
		//DebugDraw.Text3DKeyed("2test", "asd", Vector3.Zero, 1, Colors.Bisque);
		//DebugDraw.Arrow(Vector3.Zero, Vector3.Forward, 1, Colors.Aqua, 1);
		//DebugDraw.Axes(Vector3.Zero, Quaternion.Identity, 1, 1);
		//DebugDraw.Box(Vector3.Zero, Vector3.One);
		
		// DebugPlayer.Instance.Debug($"Performance.GetMonitor");
		// DebugPlayer.Instance.Debug($"TimeFps: {Performance.GetMonitor(Performance.Monitor.TimeFps)}");
		// DebugPlayer.Instance.Debug($"RenderTotalDrawCallsInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame)}");
		// DebugPlayer.Instance.Debug($"RenderTotalPrimitivesInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalPrimitivesInFrame)}");
		// DebugPlayer.Instance.Flush();
	}
}
