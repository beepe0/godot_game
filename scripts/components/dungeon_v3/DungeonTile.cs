using BP.GameConsole;
using Godot;
using Godot.Collections;

public partial class DungeonTile : Node3D
{
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
                Connectors.Add(node is Node3D ? node as Node3D : null);
            }
        }
    }
}