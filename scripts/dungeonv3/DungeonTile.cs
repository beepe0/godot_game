using Godot;
using Godot.Collections;

public partial class DungeonTile : Node3D
{
    public Node3D[] Connectors;

    public override void _Ready()
    {
        Array<Node> connectors = GetNodeOrNull<Node3D>("Connectors").GetChildren();

        if (connectors != null && connectors.Count > 0)
        {
            foreach (var node in connectors)
            {
                connectors.Add(node);
            }
        }
    }
}