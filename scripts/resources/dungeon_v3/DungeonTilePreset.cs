using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class DungeonTilePreset : Resource
{
    [ExportGroup("Tile Property Settings")]
    [Export] public string TilesCategory;
    [Export(PropertyHint.Range, "0, 100, 0.01f")] public float PercentageSpawnOfTile;

    [ExportGroup("Tile Path Settings")]
    [Export(PropertyHint.Dir)] public string RoomsFolderPath { get; set; }

}
