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
		Gizmos.Text("Performance:", 0, Colors.Burlywood);
		Gizmos.Text($"TimeFps: {Performance.GetMonitor(Performance.Monitor.TimeFps)}");
		Gizmos.Text($"TimeProcess: {Math.Round(Performance.GetMonitor(Performance.Monitor.TimeProcess) * 1000, 2)} ms");
		Gizmos.Text($"TimePhysicsProcess: {Math.Round(Performance.GetMonitor(Performance.Monitor.TimePhysicsProcess) * 1000, 2)} ms");
		Gizmos.Text($"TimeNavigationProcess: {Math.Round(Performance.GetMonitor(Performance.Monitor.TimeNavigationProcess) * 1000, 2)} ms");
		Gizmos.Text("Draw:", 0, Colors.Burlywood);
		Gizmos.Text($"RenderTotalDrawCallsInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame)}");
		Gizmos.Text($"RenderTotalPrimitivesInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalPrimitivesInFrame)}");
		Gizmos.Text("Objects", 0, Colors.Burlywood);
		Gizmos.Text($"ObjectCount: {Performance.GetMonitor(Performance.Monitor.ObjectCount)}");
		Gizmos.Text($"ObjectResourceCount: {Performance.GetMonitor(Performance.Monitor.ObjectResourceCount)}");
		Gizmos.Text($"ObjectNodeCount: {Performance.GetMonitor(Performance.Monitor.ObjectNodeCount)}");
		Gizmos.Text($"ObjectOrphanNodeCount: {Performance.GetMonitor(Performance.Monitor.ObjectOrphanNodeCount)}");
    }
}
