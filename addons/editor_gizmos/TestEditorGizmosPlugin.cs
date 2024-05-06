#if TOOLS
using Godot;
using System;
using Godot.Collections;

[Tool]
public partial class TestEditorGizmosPlugin : EditorPlugin
{
	private TestEditorGizmos _testEditorGizmos;

	public override void _EnterTree()
	{
		//AddNode3DGizmoPlugin(_testEditorGizmos = new TestEditorGizmos());
	}
	public override void _ExitTree()
	{
		//RemoveNode3DGizmoPlugin(_testEditorGizmos);
	}
	public override void _EnablePlugin()
	{
		AddNode3DGizmoPlugin(_testEditorGizmos = new TestEditorGizmos());
		SetInputEventForwardingAlwaysEnabled();
		SetForceDrawOverForwardingEnabled();
	}
	public override void _DisablePlugin()
	{
		RemoveNode3DGizmoPlugin(_testEditorGizmos);
	}
	public override bool _Handles(GodotObject @object)
	{
		GD.Print("_Handles");
		_testEditorGizmos.SelectedNodes = EditorInterface.Singleton.GetSelection().GetSelectedNodes();
		return base._Handles(@object);
	}
	public override void _ApplyChanges()
	{
		GD.Print("_ApplyChanges");
		base._ApplyChanges();
	}
	public override int _Forward3DGuiInput(Camera3D viewportCamera, InputEvent @event)
	{
		if (_testEditorGizmos.SelectedNodes.Count < 0 || !_testEditorGizmos._HasGizmo(_testEditorGizmos.SelectedNodes[0] as Node3D)) return base._Forward3DGuiInput(viewportCamera, @event);
			
		_testEditorGizmos.Сamera3D = viewportCamera;
		
		using (InputEventMouseMotion inputEventMouseMotion = @event as InputEventMouseMotion)
		{
			if (inputEventMouseMotion != null && Input.IsMouseButtonPressed(MouseButton.Middle))
			{
				UpdateOverlays();
				return (int)AfterGuiInput.Pass;
			}
		}

		using (InputEventMouseButton inputEventMouseButton = @event as InputEventMouseButton)
		{
			if (inputEventMouseButton != null && inputEventMouseButton.IsPressed() && inputEventMouseButton.ButtonIndex == MouseButton.Left)
			{
				_testEditorGizmos.Anchors.Add(viewportCamera.ProjectPosition(viewportCamera.GetViewport().GetMousePosition(), ((Node3D)_testEditorGizmos.SelectedNodes[0]).GlobalPosition.DistanceTo(viewportCamera.GlobalPosition)));
				UpdateOverlays();

				return (int)AfterGuiInput.Stop;
			}
		}

		return base._Forward3DGuiInput(viewportCamera, @event);
	}
	public override void _Forward3DDrawOverViewport(Control viewportControl)
	{
		GD.Print("_Forward3DDrawOverViewsport");
	}
	public override void _Forward3DForceDrawOverViewport(Control viewportControl)
	{
		foreach (var anchor in _testEditorGizmos.Anchors)
		{
			viewportControl.DrawCircle(_testEditorGizmos.Сamera3D.UnprojectPosition(anchor), 4, Colors.White);
			((ConvexPolygonShape3D)(_testEditorGizmos.SelectedNodes[0] as CollisionShape3D).Shape).Points = _testEditorGizmos.Anchors.ToArray();
		}
	}
}
#endif
