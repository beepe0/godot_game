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
		Gizmos.Text("Performance.GetMonitor", 0, Colors.Aqua);
		Gizmos.Text($"TimeFps: {Performance.GetMonitor(Performance.Monitor.TimeFps)}");
		Gizmos.Text($"RenderTotalDrawCallsInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame)}");
		Gizmos.Text($"RenderTotalPrimitivesInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalPrimitivesInFrame)}");
		Gizmos.Text("====================CanvasDrawer=====================", 0, Colors.Red);
		Gizmos.Text($"Gizmos._canvasDrawer.GizmosTextPool.MaxSize: {Gizmos._canvasDrawer.GizmosTextPool.MaxSize}");
		Gizmos.Text($"Gizmos._canvasDrawer.GizmosTextPool.AvailableInstances: {Gizmos._canvasDrawer.GizmosTextPool.AvailableInstances}");
		Gizmos.Text($"Gizmos._canvasDrawer.GizmosTextPool.CurrentNumberOfInstances: {Gizmos._canvasDrawer.GizmosTextPool.CurrentNumberOfInstances}");
		Gizmos.Text($"Gizmos._canvasDrawer.GizmosText2dPool.MaxSize: {Gizmos._canvasDrawer.GizmosText2dPool.MaxSize}");
		Gizmos.Text($"Gizmos._canvasDrawer.GizmosText2dPool.AvailableInstances: {Gizmos._canvasDrawer.GizmosText2dPool.AvailableInstances}");
		Gizmos.Text($"Gizmos._canvasDrawer.GizmosText2dPool.CurrentNumberOfInstances: {Gizmos._canvasDrawer.GizmosText2dPool.CurrentNumberOfInstances}");
		Gizmos.Text($"Gizmos._canvasDrawer.GizmosText3dPool.MaxSize: {Gizmos._canvasDrawer.GizmosText3dPool.MaxSize}");
		Gizmos.Text($"Gizmos._canvasDrawer.GizmosText3dPool.AvailableInstances: {Gizmos._canvasDrawer.GizmosText3dPool.AvailableInstances}");
		Gizmos.Text($"Gizmos._canvasDrawer.GizmosText3dPool.CurrentNumberOfInstances: {Gizmos._canvasDrawer.GizmosText3dPool.CurrentNumberOfInstances}");
		Gizmos.Text("====================MeshDrawer=====================", 0, Colors.Red);
		Gizmos.Text($"Gizmos._meshDrawer.GizmosMeshPool.MaxSize: {Gizmos._meshDrawer.GizmosMeshPool.MaxSize}");
		Gizmos.Text($"Gizmos._meshDrawer.GizmosMeshPool.AvailableInstances: {Gizmos._meshDrawer.GizmosMeshPool.AvailableInstances}");
		Gizmos.Text($"Gizmos._meshDrawer.GizmosMeshPool.CurrentNumberOfInstances: {Gizmos._meshDrawer.GizmosMeshPool.CurrentNumberOfInstances}");
		
		Gizmos.Text3D("Performance.GetMonitor", new Vector3(0,0,0), 0, Colors.Coral);
		Gizmos.Text3D($"TimeFps: {Performance.GetMonitor(Performance.Monitor.TimeFps)}", new Vector3(0,-0.1f,0));
		Gizmos.Text3D($"RenderTotalDrawCallsInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame)}", new Vector3(0,-0.2f,0));
		Gizmos.Text3D($"RenderTotalPrimitivesInFrame: {Performance.GetMonitor(Performance.Monitor.RenderTotalPrimitivesInFrame)}", new Vector3(0,-0.3f,0));
		
		Gizmos.Box(new Vector3(0, 0, 0), Quaternion.Identity, Vector3.One, 0, Colors.Aqua);
		Gizmos.Arrow(new Vector3(0, 3, 0), Quaternion.Identity, Vector3.One, 0, Colors.Aquamarine);
		Gizmos.Circle(new Vector3(0, 6, 0), Quaternion.Identity, Vector3.One, 0, Colors.RebeccaPurple);
		Gizmos.Plane(new Vector3(0, 9, 0), Quaternion.Identity, Vector3.One, 0, Colors.AliceBlue);
		Gizmos.Sphere(new Vector3(0, 12, 0), Quaternion.Identity, Vector3.One, 0, Colors.Red);
		Gizmos.Capsule(new Vector3(0, 16, 0), Quaternion.Identity, Vector3.One, 0, Colors.Blue);
		Gizmos.Cylinder(new Vector3(0, 21, 0), Quaternion.Identity, Vector3.One, 0, Colors.Pink);
		Gizmos.Point(new Vector3(0, 25, 0), Quaternion.Identity, Vector3.One, 0, Colors.Gainsboro);
		
		Gizmos.SolidBox(new Vector3(3, 0, 0), Quaternion.Identity, Vector3.One, 0, Colors.Salmon);
		Gizmos.Arrows(new Vector3(3, 3, 0), Quaternion.Identity, Vector3.One, 0);
		Gizmos.SolidCapsule(new Vector3(3, 6, 0), Quaternion.Identity, Vector3.One, 0, Colors.Bisque);
		Gizmos.SolidCylinder(new Vector3(3, 11, 0), Quaternion.Identity, Vector3.One, 0, Colors.ForestGreen);
		Gizmos.SolidSphere(new Vector3(3, 15, 0), Quaternion.Identity, Vector3.One, 0, Colors.Gold);
    }
}
