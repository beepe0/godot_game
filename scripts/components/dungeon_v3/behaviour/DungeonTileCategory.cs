using System.Collections.Generic;
using System.Threading.Tasks;
using BP.ComponentSystem;
using BP.GameConsole;
using Godot;
using System;

public partial class DungeonTileCategory : Node
{
    [Export] public DungeonTileCategoryPreset DungeonTileCategoryPreset;
    
    public DungeonBuilder DungeonBuilder;
    public readonly List<PackedScene> PackedScenes = new ();
    public ushort CurrentNumberOfTilesPerTier;
    
    public List<DungeonTileCategory> _availableTileScenes = new ();

    public override void _Ready()
    {
        try
        {
            DungeonBuilder = ComponentSystem.GetComponentSystemWithTag("Dungeon").GetComponent<DungeonBuilder>();

            foreach (string e in DungeonTileCategoryPreset.ValidNeighbours)
            {
                _availableTileScenes.Add(DungeonBuilder.TileScenes[e]);
            }
        }
        catch (Exception e)
        {
            GameConsole.Instance.DebugLog($"{GetType()} :: {e}");
        }
    }
    public async Task<List<DungeonTile>> Execute(DungeonTile tile)
    {
        List<DungeonTile> neighbours = new ();
        foreach (Node3D targetConnector in tile.Connectors)
        {
            if(DungeonBuilder.CurrentNumberOfRooms >= DungeonBuilder.ResLoader.DungeonBuilderPreset.NumberOfRooms) break;
                
            DungeonTile currentRoom = await InitTileOrNull(targetConnector);
                
            if(currentRoom != null) neighbours.Add(currentRoom);
        }
        return neighbours;
    }

    private async Task<DungeonTile> InitTileOrNull(Node3D targetSnap = null)
    {
        if (_availableTileScenes.Count < 1) return null;

        DungeonTile tile = InitTileWithMin(_availableTileScenes, out var tileProperties);

        if (targetSnap != null)
        {
            Node3D currentConnector = tile.Connectors[(ushort)DungeonBuilder.Random.RandiRange(0, tile.Connectors.Count - 1)];
            tile.Snap(currentConnector, targetSnap);
                
            await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
                
            if (tile.Bounds.GetOverlappingAreas().Count > 0)
            {
                tile.QueueFree();
                return null;
            }
        
            currentConnector.Visible = false;
            targetSnap.Visible = false;
        }
        
        if (tileProperties.CurrentNumberOfTilesPerTier >= tileProperties.DungeonTileCategoryPreset.TargetNumberOfTilesPerTier - 1)
        {
            _availableTileScenes.Remove(tileProperties);
            // _unavailableTileScenes.Add(tileProperties);
        }
                    
        tileProperties.CurrentNumberOfTilesPerTier++;
        DungeonBuilder.CurrentNumberOfRooms++;
        
        return tile;
    }

    private DungeonTile InitTileWithMin(List<DungeonTileCategory> list, out DungeonTileCategory tileProperties)
    {
        return (tileProperties = GetMin(list)).PackedScenes[(ushort)DungeonBuilder.Random.RandiRange(0, tileProperties.PackedScenes.Count - 1)].CreateOnStage<DungeonTile>(this, DungeonBuilder.ResLoader.DungeonBuilderPreset.StartPosition);
    }
    private DungeonTileCategory GetMin(List<DungeonTileCategory> list)
    {
        DungeonTileCategory minTile = list[0];
        ushort minPriority = minTile.DungeonTileCategoryPreset.Priority;

        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].DungeonTileCategoryPreset.Priority < minPriority)
            {
                minTile = list[i];
                minPriority = minTile.DungeonTileCategoryPreset.Priority;
            }
        }

        return minTile;
    }
}