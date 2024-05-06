using System.Collections.Generic;
using BP.DebugGizmos;
using Godot;
using Godot.Collections;

public partial class TestEditorGizmos : EditorNode3DGizmoPlugin
{
    private readonly GizmosShape _boxGizmosShape;
    private readonly GizmosShape _solidBoxGizmosShape;

    public readonly List<Vector3> Anchors;
    public Camera3D Сamera3D;
    public Array<Node> SelectedNodes;

    public TestEditorGizmos()
    {
        Anchors = new();
        
        GD.Print("Constructor");
        AddMaterial("selected_box", CreateStandardMaterial3D(Colors.White));
        AddMaterial("selected_solid_box", CreateStandardMaterial3D(new Color(Colors.ForestGreen, 0.4f), true, false, true));
        
        AddMaterial("unselected_box", CreateStandardMaterial3D(Colors.Burlywood));
        AddMaterial("unselected_solid_box", CreateStandardMaterial3D(new Color(Colors.DarkGray, 0.4f), true, false, false));
        
        _boxGizmosShape = new GizmosShape(Mesh.PrimitiveType.Lines, ProjectSettings.GetSetting(GizmosPlugin.MeshesProperties["box_mesh_res"].Path).ToString());
        _solidBoxGizmosShape = new GizmosShape(Mesh.PrimitiveType.Triangles, ProjectSettings.GetSetting(GizmosPlugin.MeshesProperties["solid_box_mesh_res"].Path).ToString());
    }
    private EditorNode3DGizmo CreateEditorNode3DGizmo(Node3D target)
    {
        target.SetMeta("_edit_lock_", true);
        return new EditorNode3DGizmo();
    }
    private StandardMaterial3D CreateStandardMaterial3D(Color color, bool blend = true, bool billboard = false, bool noDepthTest = true)
    {
        return new()
        {
            AlbedoColor = color,
            ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
            BlendMode = blend ? BaseMaterial3D.BlendModeEnum.Add : BaseMaterial3D.BlendModeEnum.Mix,
            VertexColorUseAsAlbedo = true,
            Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
            BillboardMode = billboard ? BaseMaterial3D.BillboardModeEnum.Enabled : BaseMaterial3D.BillboardModeEnum.Disabled,
            BillboardKeepScale = billboard,
            NoDepthTest = noDepthTest,
            CullMode = BaseMaterial3D.CullModeEnum.Back
        };
    }
    public override void _Redraw(EditorNode3DGizmo gizmo)
    {
        gizmo.Clear();
        
        if (SelectedNodes.Count < 0 || !_HasGizmo(SelectedNodes[0] as Node3D))
        {
            gizmo.AddMesh(_solidBoxGizmosShape.LoadedMesh, GetMaterial("selected_solid_box"));
            gizmo.AddMesh(_boxGizmosShape.GizmosMesh, GetMaterial("selected_box"));
            gizmo.AddHandles(Anchors.ToArray(), GetMaterial("selected_box"), null);
        }
        else
        {
            gizmo.AddMesh(_solidBoxGizmosShape.LoadedMesh, GetMaterial("unselected_solid_box"));
            gizmo.AddMesh(_boxGizmosShape.GizmosMesh, GetMaterial("unselected_box"));
        }
    }
    public override void _SetHandle(EditorNode3DGizmo gizmo, int handleId, bool secondary, Camera3D camera, Vector2 screenPos)
    {
        GD.Print("Handle " + gizmo.GetPlugin()._GetGizmoName());
    }
    public override void _CommitHandle(EditorNode3DGizmo gizmo, int handleId, bool secondary, Variant restore, bool cancel)
    {
        GD.Print(gizmo.GetPlugin()._GetGizmoName());
    }
    public override EditorNode3DGizmo _CreateGizmo(Node3D forNode3D)
    {
        return _HasGizmo(forNode3D) ? CreateEditorNode3DGizmo(forNode3D) : null;
    }
    public override bool _HasGizmo(Node3D forNode3D)
    {
        return (forNode3D as CollisionShape3D)?.Shape is ConvexPolygonShape3D;
    }
    public override string _GetGizmoName()
    {
        return "TestEditorGizmos";
    }
}