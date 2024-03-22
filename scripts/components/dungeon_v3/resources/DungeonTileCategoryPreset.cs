using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class DungeonTileCategoryPreset : Resource
{
    [ExportGroup("Tile Property Settings")] 
    [Export] public string TilesCategory { get; set; }
    [Export] public ushort TargetNumberOfTilesPerTier { get; set; }
    [Export] public ushort Priority { get; set; }
    [Export] public string[] ValidNeighbours { get; set; }

    [ExportGroup("Tile Path Settings")]
    [Export(PropertyHint.Dir)] public string RoomsFolderPath { get; set; }
}
