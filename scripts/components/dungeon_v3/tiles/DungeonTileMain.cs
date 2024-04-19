using BP.DebugGizmos;
using Godot;
using Godot.Collections;

public partial class DungeonTileMain : DungeonTile
{
    // [Export] public Array<Node3D> SecondConnectors;
    // public bool CanBeAdded;
    public override void DrawGizmos(ushort id)
    {
        base.DrawGizmos(id);

        Gizmos.Text3D($"ID: {id} - {Preset.TilesCategory}", GlobalPosition);
        Gizmos.SolidSphere(GlobalPosition, Quaternion.Identity, Vector3.One / 2, 0, Colors.Red);
    }
}