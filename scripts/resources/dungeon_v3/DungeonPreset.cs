using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class DungeonPreset : Resource
{
    [ExportGroup("Dungeon Property Settings")]
    [Export] public ulong Seed { get; set; }
    [Export] public ushort NumberOfRooms { get; set; }
    [Export] public Vector3 StartPosition { get; set; }

    [ExportGroup("Tiles Path Settings")]
    [Export] public Array<DungeonTilePreset> TileSences = new();
}
