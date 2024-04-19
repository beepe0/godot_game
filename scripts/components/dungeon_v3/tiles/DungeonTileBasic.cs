using BP.DebugGizmos;
using BP.GameConsole;
using Godot;
using Godot.Collections;

public partial class DungeonTileBasic : DungeonTile
{
    public override void DrawGizmos(ushort id)
    {
        base.DrawGizmos(id);

        Gizmos.Text3D($"ID: {id} - {Preset.TilesCategory}", GlobalPosition);
        Gizmos.SolidSphere(GlobalPosition, Quaternion.Identity, Vector3.One / 2, 0, Colors.Green);
    }
}