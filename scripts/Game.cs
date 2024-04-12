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
		DrawGizmos.Text("Performance.GetMonitor", 0, Colors.Aqua);
		DrawGizmos.Text($"TimeFps: {Performance.GetMonitor(Performance.Monitor.TimeFps)}");
		DrawGizmos.Text($"RenderTotalDrawCallsInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame)}");
		DrawGizmos.Text($"RenderTotalPrimitivesInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalPrimitivesInFrame)}");
		DrawGizmos.Text("====================CanvasDrawer=====================", 0, Colors.Red);
		DrawGizmos.Text($"DrawGizmos._canvasDrawer.GizmosTextPool.MaxSize: {DrawGizmos._canvasDrawer.GizmosTextPool.MaxSize}");
		DrawGizmos.Text($"DrawGizmos._canvasDrawer.GizmosTextPool.AvailableInstances: {DrawGizmos._canvasDrawer.GizmosTextPool.AvailableInstances}");
		DrawGizmos.Text($"DrawGizmos._canvasDrawer.GizmosTextPool.CurrentNumberOfInstances: {DrawGizmos._canvasDrawer.GizmosTextPool.CurrentNumberOfInstances}");
		DrawGizmos.Text($"DrawGizmos._canvasDrawer.GizmosText2dPool.MaxSize: {DrawGizmos._canvasDrawer.GizmosText2dPool.MaxSize}");
		DrawGizmos.Text($"DrawGizmos._canvasDrawer.GizmosText2dPool.AvailableInstances: {DrawGizmos._canvasDrawer.GizmosText2dPool.AvailableInstances}");
		DrawGizmos.Text($"DrawGizmos._canvasDrawer.GizmosText2dPool.CurrentNumberOfInstances: {DrawGizmos._canvasDrawer.GizmosText2dPool.CurrentNumberOfInstances}");
		DrawGizmos.Text($"DrawGizmos._canvasDrawer.GizmosText3dPool.MaxSize: {DrawGizmos._canvasDrawer.GizmosText3dPool.MaxSize}");
		DrawGizmos.Text($"DrawGizmos._canvasDrawer.GizmosText3dPool.AvailableInstances: {DrawGizmos._canvasDrawer.GizmosText3dPool.AvailableInstances}");
		DrawGizmos.Text($"DrawGizmos._canvasDrawer.GizmosText3dPool.CurrentNumberOfInstances: {DrawGizmos._canvasDrawer.GizmosText3dPool.CurrentNumberOfInstances}");
		DrawGizmos.Text("====================MeshDrawer=====================", 0, Colors.Red);
		DrawGizmos.Text($"DrawGizmos._meshDrawer.GizmosMeshPool.MaxSize: {DrawGizmos._meshDrawer.GizmosMeshPool.MaxSize}");
		DrawGizmos.Text($"DrawGizmos._meshDrawer.GizmosMeshPool.AvailableInstances: {DrawGizmos._meshDrawer.GizmosMeshPool.AvailableInstances}");
		DrawGizmos.Text($"DrawGizmos._meshDrawer.GizmosMeshPool.CurrentNumberOfInstances: {DrawGizmos._meshDrawer.GizmosMeshPool.CurrentNumberOfInstances}");
		
		DrawGizmos.Text3D("Performance.GetMonitor", new Vector3(0,0,0), 0, Colors.Coral);
		DrawGizmos.Text3D($"TimeFps: {Performance.GetMonitor(Performance.Monitor.TimeFps)}", new Vector3(0,-0.1f,0));
		DrawGizmos.Text3D($"RenderTotalDrawCallsInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame)}", new Vector3(0,-0.2f,0));
		DrawGizmos.Text3D($"RenderTotalPrimitivesInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalPrimitivesInFrame)}", new Vector3(0,-0.3f,0));
		
		DrawGizmos.Box(new Vector3(0, 0, 0), Quaternion.Identity, Vector3.One, 0, Colors.Aqua);
		DrawGizmos.Arrow(new Vector3(0, 3, 0), Quaternion.Identity, Vector3.One, 0, Colors.Aquamarine);
		DrawGizmos.Circle(new Vector3(0, 6, 0), Quaternion.Identity, Vector3.One, 0, Colors.RebeccaPurple);
		DrawGizmos.Plane(new Vector3(0, 9, 0), Quaternion.Identity, Vector3.One, 0, Colors.AliceBlue);
		DrawGizmos.Sphere(new Vector3(0, 12, 0), Quaternion.Identity, Vector3.One, 0, Colors.Red);
		DrawGizmos.Capsule(new Vector3(0, 16, 0), Quaternion.Identity, Vector3.One, 0, Colors.Blue);
		DrawGizmos.Cylinder(new Vector3(0, 21, 0), Quaternion.Identity, Vector3.One, 0, Colors.Pink);
		DrawGizmos.Point(new Vector3(0, 25, 0), Quaternion.Identity, Vector3.One, 0, Colors.Gainsboro);
		
		DrawGizmos.SolidBox(new Vector3(3, 0, 0), Quaternion.Identity, Vector3.One, 0, Colors.Salmon);
		DrawGizmos.Arrows(new Vector3(3, 3, 0), Quaternion.Identity, Vector3.One, 0);
		DrawGizmos.SolidCapsule(new Vector3(3, 6, 0), Quaternion.Identity, Vector3.One, 0, Colors.Bisque);
		DrawGizmos.SolidCylinder(new Vector3(3, 11, 0), Quaternion.Identity, Vector3.One, 0, Colors.ForestGreen);
		DrawGizmos.SolidSphere(new Vector3(3, 15, 0), Quaternion.Identity, Vector3.One, 0, Colors.Gold);
		
    }
}
