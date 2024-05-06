using BP.DebugGizmos;
using Godot;
using Godot.Collections;

public partial class DungeonTileFinish : DungeonTile
{
    [Export] public Array<Node3D> SecondConnectors;
    public bool CanBeAdded;
    public override void OnDrawGizmos(ushort id)
    {
        base.OnDrawGizmos(id);
        Gizmos.Text3D($"ID: {id} - {Preset.TilesCategory}", GlobalPosition);
        Gizmos.SolidSphere(GlobalPosition, Quaternion.Identity, Vector3.One / 2, 0, Colors.Aqua);
    }
    public override void OnInit()
    {
        base.OnInit();
        Connectors.Clear();
        Connectors.AddRange(SecondConnectors);
        
        DungeonBuilder.DungeonTiers[DungeonBuilder.CurrentNumberOfTiers].ValidTilesFrom.Add(this);
    }
}