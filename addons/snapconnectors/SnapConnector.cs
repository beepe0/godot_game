#if TOOLS
using Godot;
using System;

[Tool]
public partial class SnapConnector : EditorPlugin
{
    private Node3D bestFirstConnector = null;
    private Node3D bestSecondConnector = null;
    private Node3D selectedNode = null;
    private Node3D snapNode = null;

    private Node3D selectedFirstNodeConnectors = null;
    private Node3D selectedSecondNodeConnectors = null;
    public override void _EnterTree()
    {
    }
    public override void _ExitTree()
    {
    }
    public override void _Process(double delta)
    {
        //GD.Print($"Connector: ");

        if(!Input.IsActionJustReleased("ui_accept")) return;
        
        var selection = GetEditorInterface().GetSelection();
        if (selection.GetSelectedNodes().Count > 0)
        {
            var groupedNodes = GetTree().GetNodesInGroup("DungeonTile");
            if(groupedNodes.Count < 1) return;
            
            selectedNode = selection.GetSelectedNodes()[0] as Node3D;

            selectedFirstNodeConnectors = selectedNode?.GetNodeOrNull<Node3D>("Connectors");
            
            if (selectedFirstNodeConnectors != null)
            {    
                float closestDistance = float.PositiveInfinity;

                foreach (Node3D cn in groupedNodes)
                {
                    if(cn.Equals(selectedNode)) continue;
                    selectedSecondNodeConnectors = cn?.GetNodeOrNull<Node3D>("Connectors");
                    foreach (Node3D sn in selectedSecondNodeConnectors.GetChildren())
                    {
                        foreach (Node3D fn in selectedFirstNodeConnectors.GetChildren())
                        {
                            float distance = (fn.GlobalPosition - sn.GlobalPosition).Length();                      

                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                bestFirstConnector = fn;
                                bestSecondConnector = sn;
                                snapNode = cn;
                            }
                        }
                    }
                }

                GD.Print($"Tile: {snapNode.Name}, ClosestTile: {closestDistance}, NFN: {bestFirstConnector.GlobalBasis.Z.Normalized()}, NSN: {bestSecondConnector.GlobalBasis.Z.Normalized()} ");

                if (closestDistance < 3f)
                {
                    selectedNode.Rotation = Vector3.Zero;
                    selectedNode.Position = Vector3.Zero;
                    selectedNode.Position = bestSecondConnector.GlobalPosition - (bestFirstConnector.GlobalPosition - selectedNode.Position);
                    Vector2 vectorA = new Vector2(bestFirstConnector.GlobalBasis.Z.X, bestFirstConnector.GlobalBasis.Z.Z);
                    Vector2 vectorB = new Vector2(bestSecondConnector.GlobalBasis.Z.X, bestSecondConnector.GlobalBasis.Z.Z);
                    float rawAngle = (vectorA).AngleTo(vectorB);
                    float angle = Mathf.Pi - rawAngle;
                    
                    selectedNode.Position = bestSecondConnector.GlobalPosition + (selectedNode.Position - bestSecondConnector.GlobalPosition).Rotated(Vector3.Up, angle);
                    selectedNode.Rotation = Vector3.Up * angle;
                    GD.Print($"Tile: {snapNode.Name}, closestDistance: {closestDistance}, angle: {Mathf.RadToDeg(angle)}, rawAngle: {Mathf.RadToDeg(rawAngle)}, rad angle: {angle}, rad rawAngle: {rawAngle}");
                }
            }
        }
    }
}
#endif