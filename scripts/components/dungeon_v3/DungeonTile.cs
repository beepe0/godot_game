using System;
using BP.GameConsole;
using Godot;
using Godot.Collections;

public partial class DungeonTile : Node3D
{
    [Export] public DungeonTileCategory TileCategory;
    public Array<Node3D> Connectors;
    public Area3D Bounds;

    public override void _Ready()
    {
        Bounds = GetNodeOrNull<Area3D>("Bounds");
        
        Array<Node> connectors = GetNodeOrNull<Node3D>("Connectors").GetChildren();
        if (connectors != null && connectors.Count > 0)
        {
            Connectors = new Array<Node3D>();

            foreach (var node in connectors)
            {
                Connectors.Add(node as Node3D);
            }
        }
    }
    public void Snap(Node3D currentConnector, Node3D targetConnector)
    {
        if(this.Connectors.Count < 1) return;
        
        this.Rotation = Vector3.Zero;
        
        this.Position = targetConnector.GlobalPosition - (currentConnector.GlobalPosition - this.Position);
        Vector2 vectorA = new Vector2(currentConnector.GlobalBasis.Z.X, currentConnector.GlobalBasis.Z.Z);
        Vector2 vectorB = new Vector2(targetConnector.GlobalBasis.Z.X, targetConnector.GlobalBasis.Z.Z);
        float rawAngle = (vectorA).AngleTo(vectorB);
        float angle = Mathf.Pi - rawAngle;
                    
        this.Position = targetConnector.GlobalPosition + (this.Position - targetConnector.GlobalPosition).Rotated(Vector3.Up, angle);
        this.Rotation = Vector3.Up * angle;
    }
    public enum DungeonTileCategory : byte
    {
        Basic,
        Main,
        Finish,
    }
}