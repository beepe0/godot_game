using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class DungeonTileCategoryPreset : Resource
{
    [ExportGroup("Tile Property Settings")] 
    [Export] public string GenerationName { get; set; }
    [Export] public string TilesCategory { get; set; }
    [Export] public ushort TargetNumberOfTilesPerTier { get; set; }
    [Export] public ushort Priority { get; set; }
    [Export] public string[] ValidNeighbours { get; set; }
    
    public readonly List<PackedScene> PackedScenes = new();
    public readonly List<DungeonTileCategoryPreset> AvailableTileScenes = new();
    public readonly List<DungeonTileCategoryPreset> UnavailableTileScenes = new();
    public ushort CurrentNumberOfTilesPerTier;
    
    [ExportGroup("Tile Path Settings")]
    [Export(PropertyHint.Dir)] public string RoomsFolderPath { get; set; }

    public void ResetTempProperties()
    {
        AvailableTileScenes.AddRange(UnavailableTileScenes);
        UnavailableTileScenes.Clear();
        CurrentNumberOfTilesPerTier = 0;
    }
}
