using System;
using System.Diagnostics;
using BP.DebugGizmos;
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
		DrawGizmos.Text("Performance.GetMonitor", 0.3f, Colors.Aqua);
		DrawGizmos.Text($"TimeFps: {Performance.GetMonitor(Performance.Monitor.TimeFps)}");
		DrawGizmos.Text($"RenderTotalDrawCallsInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame)}");
		DrawGizmos.Text($"RenderTotalPrimitivesInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalPrimitivesInFrame)}");
		
		DrawGizmos.Text3D("Performance.GetMonitor", new Vector3(0,0,0), 0, Colors.Coral);
		DrawGizmos.Text3D($"TimeFps: {Performance.GetMonitor(Performance.Monitor.TimeFps)}", new Vector3(0,-0.1f,0));
		DrawGizmos.Text3D($"RenderTotalDrawCallsInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame)}", new Vector3(0,-0.2f,0));
		DrawGizmos.Text3D($"RenderTotalPrimitivesInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalPrimitivesInFrame)}", new Vector3(0,-0.3f,0));
		// DebugDraw.Text("Performance.GetMonitor", 0.15f, Colors.Coral);
		// DebugDraw.Text("Performance.GetMonitor", 0.15f, Colors.Aqua);
		// DebugDraw.Text($"TimeFps: {Performance.GetMonitor(Performance.Monitor.TimeFps)}");
		// DebugDraw.Text($"RenderTotalDrawCallsInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame)}");
		// DebugDraw.Text($"RenderTotalPrimitivesInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalPrimitivesInFrame)}");
		// DebugDraw.Text("================================================");
		// DebugDraw.Text("================================================");
		// DebugDraw.Text("================================================");
		// DebugDraw.Text($"DebugDraw._canvasDrawer.TextPool._pool.Count: {DebugDraw._canvasDrawer.TextPool._pool.Count}");
		// DebugDraw.Text($"DebugDraw._canvasDrawer.Text3DPool._pool.Count: {DebugDraw._canvasDrawer.Text3DPool._pool.Count}");
		// DebugDraw.Text("================================================");
		// DebugDraw.Text($"DebugDraw._canvasDrawer._textEntries.Count: {DebugDraw._canvasDrawer._textEntries.Count}");
		// DebugDraw.Text($"DebugDraw._canvasDrawer._text3dEntries.Count: {DebugDraw._canvasDrawer._text3dEntries.Count}");
		//
		// DebugDraw.Text3D("Performance.GetMonitor", new Vector3(1,0,0), 0, Colors.RosyBrown);
		// DebugDraw.Text3D($"TimeFps: {Performance.GetMonitor(Performance.Monitor.TimeFps)}", new Vector3(1,-0.1f,0));
		// DebugDraw.Text3D($"RenderTotalDrawCallsInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame)}", new Vector3(1,-0.2f,0));
		// DebugDraw.Text3D($"RenderTotalPrimitivesInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalPrimitivesInFrame)}", new Vector3(1,-0.3f,0));
		
		// DebugPlayer.Instance.Debug($"Performance.GetMonitor");
		// DebugPlayer.Instance.Debug($"TimeFps: {Performance.GetMonitor(Performance.Monitor.TimeFps)}");
		// DebugPlayer.Instance.Debug($"RenderTotalDrawCallsInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame)}");
		// DebugPlayer.Instance.Debug($"RenderTotalPrimitivesInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalPrimitivesInFrame)}");
		// DebugPlayer.Instance.Flush();
    }
}
