using BP.DebugGizmos;
using Godot;
using Godot.Collections;

public partial class DungeonTileMain : DungeonTile
{
	public override void OnDrawGizmos(ushort id)
	{
		base.OnDrawGizmos(id);

		Gizmos.Text3D($"ID: {id} - {Preset.TilesCategory}", GlobalPosition);
		Gizmos.SolidSphere(GlobalPosition, Quaternion.Identity, Vector3.One / 2, 0, Colors.Red);
	}
}
