using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class DungeonCategoryPreset : Resource
{
    [ExportGroup("Tile Property Settings")]
    [Export] public DungeonTile.DungeonTileCategory TilesCategory { get; set; }
    [Export] public ushort TargetNumberOfTilesPerTier { get; set; }
    [Export] public ushort Priority { get; set; }

    [ExportGroup("Tile Path Settings")]
    [Export(PropertyHint.Dir)] public string RoomsFolderPath { get; set; }
}
