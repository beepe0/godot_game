using BP.DebugGizmos;
using BP.GameConsole;
using Godot;
using Godot.Collections;

public partial class DungeonTileBasic : DungeonTile
{
    public override void OnDrawGizmos(ushort id)
    {
        base.OnDrawGizmos(id);

        Gizmos.Text3D($"ID: {id} - {Preset.TilesCategory}", GlobalPosition);
        Gizmos.SolidSphere(GlobalPosition, Quaternion.Identity, Vector3.One / 2, 0, Colors.Green);
    }
}