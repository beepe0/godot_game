using System;
using System.Reflection.Emit;
using BP.GameConsole;
using Godot;
using Array = Godot.Collections.Array;
using Label = Godot.Label;

public partial class Gizmo3D : Node
{
    private MultiMeshInstance3D _multiMeshInstance;
    private Node2D _canvas2d;
    private Font _font;
    private static readonly Vector3[] CubeVertices =
    {
        new(-0.5f, -0.5f, -0.5f),
        new(0.5f, -0.5f, -0.5f),
        new(0.5f, 0.5f, -0.5f),
        new(-0.5f, 0.5f, -0.5f),
        new(-0.5f, -0.5f, 0.5f),
        new(0.5f, -0.5f, 0.5f),
        new(0.5f, 0.5f, 0.5f),
        new(-0.5f, 0.5f, 0.5f)
    };

    private static readonly int[] CubeIndices =
    {
		//top
		0, 1,
        1, 2,
        2, 3,
        3, 0,

		//bottom
		4, 5,
        5, 6,
        6, 7,
        7, 4,

		//edges
		0, 4,
        1, 5,
        2, 6,
        3, 7
    };
    public static Mesh CreateBoxMesh(Mesh.PrimitiveType type = Mesh.PrimitiveType.Lines)
    {
        var arrMesh = new ArrayMesh();

        Vector3[] vertices = null;
        int[] indices = null;
        Color[] colors = null;

        vertices = CubeVertices;
        indices = CubeIndices;
        
        Array arrays = new();
        arrays.Resize((int)Mesh.ArrayType.Max);
        arrays[(int)Mesh.ArrayType.Vertex] = vertices;
        arrays[(int)Mesh.ArrayType.Index] = indices;

        if (colors != null)
        {
            arrays[(int)Mesh.ArrayType.Color] = colors;
        }

        arrMesh.AddSurfaceFromArrays(type, arrays);

        return arrMesh;
    }
    public override void _Ready()
    {
        Mesh box = CreateBoxMesh();
        _canvas2d = new Node2D();
        
        Label label = new();
        _font = label.GetThemeDefaultFont();
        label.Free();

        _multiMeshInstance = new MultiMeshInstance3D
        {
            Name = "Box",
            CastShadow = GeometryInstance3D.ShadowCastingSetting.Off,
            GIMode = GeometryInstance3D.GIModeEnum.Disabled,
            IgnoreOcclusionCulling = true,

            MaterialOverride = new StandardMaterial3D
            {
                ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
                BlendMode = false ? BaseMaterial3D.BlendModeEnum.Add : BaseMaterial3D.BlendModeEnum.Mix,
                VertexColorUseAsAlbedo = true,
                Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
                CullMode = BaseMaterial3D.CullModeEnum.Back
            },
            Multimesh = new MultiMesh
            {
                TransformFormat = MultiMesh.TransformFormatEnum.Transform3D,
                UseColors = true,
                Mesh = box,
            }
        };
        _multiMeshInstance.Multimesh.InstanceCount = 2;

        _multiMeshInstance.Multimesh.SetInstanceTransform(0, new Transform3D(Basis.Identity, Vector3.One));
        _multiMeshInstance.Multimesh.SetInstanceColor(0, Colors.Cyan);
        _multiMeshInstance.Multimesh.SetInstanceTransform(1, new Transform3D(Basis.Identity, Vector3.Zero));
        _multiMeshInstance.Multimesh.SetInstanceColor(1, Colors.Red);

        AddChild(_multiMeshInstance);
        AddChild(_canvas2d);
    }
    public override void _Process(double delta)
    {
        Camera3D camera = _canvas2d.GetViewport().GetCamera3D();
        if(camera == null)
        {
            GameConsole.Instance.Debug("asdasldkjasd");
            return;
        }
        Vector2 offset = _font.GetStringSize("test", HorizontalAlignment.Left, -1f, 12) * 0.5f;
        Vector2 pos = camera.UnprojectPosition(new Vector3(1, 1, 1)) - offset;
        _canvas2d.DrawString(_font, pos, "test", HorizontalAlignment.Left, -1, 12, Colors.White);
    }
}