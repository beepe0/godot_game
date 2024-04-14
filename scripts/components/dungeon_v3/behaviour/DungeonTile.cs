using BP.DebugGizmos;
using Godot;
using Godot.Collections;

public partial class DungeonTile : Node3D
{
    [ExportGroup("Dungeon Tile Category")]
    [Export] public string Category = "none";
    
    [ExportGroup("Properties")]
    [Export] public bool IsAutoDetect = true;
    [Export] public Array<Node3D> Connectors;
    [Export] public Area3D Bounds;

    private TargetTest _target;

    public override void _Ready()
    {
        _target = GetTree().GetFirstNodeInGroup("TargetTest") as TargetTest;
        
        if (IsAutoDetect)
        {
            Bounds = GetNodeOrNull<Area3D>("Bounds");

            Array<Node> connectors = GetNodeOrNull<Node3D>("Connectors").GetChildren();
            if (connectors != null && connectors.Count > 0)
            {
                Connectors = new ();

                foreach (var node in connectors)
                {
                    Connectors.Add(node as Node3D);
                }
            }
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        DrawGizmos.SolidSphere(Position, Quaternion.Identity, Vector3.One, 0, Colors.Cyan);
        Visible = !(Position.DistanceTo(_target.GlobalPosition) > _target.Range);
    }
    public void Snap(Node3D currentConnector, Node3D targetConnector)
    {
        if(Connectors.Count < 1) return;
        
        Rotation = Vector3.Zero;
        
        Position = targetConnector.GlobalPosition - (currentConnector.GlobalPosition - Position);
        Vector2 vectorA = new Vector2(currentConnector.GlobalBasis.Z.X, currentConnector.GlobalBasis.Z.Z);
        Vector2 vectorB = new Vector2(targetConnector.GlobalBasis.Z.X, targetConnector.GlobalBasis.Z.Z);
        float rawAngle = (vectorA).AngleTo(vectorB);
        float angle = Mathf.Pi - rawAngle;
                    
        Position = targetConnector.GlobalPosition + (Position - targetConnector.GlobalPosition).Rotated(Vector3.Up, angle);
        Rotation = Vector3.Up * angle;
    }
}
