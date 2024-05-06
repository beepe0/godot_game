using Godot;
using System;
using BP.DebugGizmos;
using BP.GameConsole;

public partial class TestNode : Node3D
{
    [Export] private PackedScene PackedScene;
    private TestTarget _testTarget;

    public override void _Ready()
    {
        _testTarget = PackedScene.CreateOnStage<TestTarget>(this, new Vector3(0.5f,0,0));
        BoxShape3D boxShape3D = _testTarget.CollisionShape3D.Shape as BoxShape3D;
        PhysicsShapeQueryParameters3D physicsShapeQueryParameters3D = new PhysicsShapeQueryParameters3D()
        {
            CollideWithAreas = true, 
            CollideWithBodies = false,
            ShapeRid = boxShape3D.GetRid(),
            Shape = boxShape3D,
            Transform = _testTarget.CollisionShape3D.GlobalTransform,
        };
        GameConsole.Instance.DebugLog(_testTarget.CollisionShape3D.GetGizmos().Count);
        if (GetWorld3D().DirectSpaceState.IntersectShape(physicsShapeQueryParameters3D).Count > 0)
        {
            GameConsole.Instance.DebugLog("OK!");
            Gizmos.Box(_testTarget.CollisionShape3D.GlobalTransform, boxShape3D.Size, 0, Colors.Aqua);
        }
        else
        {
            Gizmos.Box(_testTarget.CollisionShape3D.GlobalTransform, boxShape3D.Size);
        }
    }
}
