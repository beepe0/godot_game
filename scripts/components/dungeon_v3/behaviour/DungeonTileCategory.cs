using System.Collections.Generic;
using System.Threading.Tasks;
using BP.ComponentSystem;
using BP.GameConsole;
using Godot;
using System;

public partial class DungeonTileCategory : Node
{
    [Export] public DungeonTileCategoryPreset DungeonTileCategoryPreset;
    public readonly List<PackedScene> PackedScenes = new ();
    public ushort CurrentNumberOfTilesPerTier;
    
    protected DungeonBuilder DungeonBuilder;
    private readonly List<DungeonTileCategory> _availableTileScenes = new ();
    private readonly List<DungeonTileCategory> _unavailableTileScenes = new ();

    public override void _Ready()
    {
        try
        {
            DungeonBuilder = ComponentSystem.GetComponentSystemWithTag("Dungeon").GetComponent<DungeonBuilder>();

            if (DungeonTileCategoryPreset.ValidNeighbours.Length > 0)
            {
                foreach (string e in DungeonTileCategoryPreset.ValidNeighbours)
                {
                    _availableTileScenes.Add(DungeonBuilder.TileScenes[e]);
                }
            }
        }
        catch (Exception e)
        {
            GameConsole.Instance.DebugLog($"{GetType()} :: {e}");
        }
    }
    public void Reset()
    {
        CurrentNumberOfTilesPerTier = 0;
        if (_unavailableTileScenes.Count > 0)
        {
            _availableTileScenes.AddRange(_unavailableTileScenes);
            _unavailableTileScenes.Clear();
        }
    }
    public virtual async Task<List<DungeonTile>> Execute(DungeonTile tile, bool isRemove)
    {
        List<DungeonTile> neighbours = new ();
        foreach (Node3D targetConnector in tile.Connectors)
        {
            if(DungeonBuilder.CurrentNumberOfTiers >= DungeonBuilder.ResLoader.DungeonBuilderPreset.NumberOfTiers) break;
                
            DungeonTile currentRoom = await InitTileOrNull(targetConnector);
                
            if(currentRoom != null) neighbours.Add(currentRoom);
        }

        if (isRemove) DungeonBuilder.ValidTiles.Remove(tile);
        
        return neighbours;
    }
    private async Task<DungeonTile> InitTileOrNull(Node3D targetSnap = null)
    {
        DungeonTileCategory tileProperties = null;
        do
        {
            if (_availableTileScenes.Count < 1) return null;

            tileProperties = GetMin(_availableTileScenes);

            if (tileProperties.CurrentNumberOfTilesPerTier >= tileProperties.DungeonTileCategoryPreset.TargetNumberOfTilesPerTier)
            {
                _availableTileScenes.Remove(tileProperties);
                _unavailableTileScenes.Add(tileProperties);
                tileProperties = null;
            }
            
        } while (tileProperties == null);
        
        DungeonTile tile = InitTile(tileProperties);
        
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
        
        tileProperties.CurrentNumberOfTilesPerTier++;
        DungeonBuilder.CurrentNumberOfTiles++;
        return tile;
    }
    private DungeonTile InitTile(DungeonTileCategory tileCategory)
    {
        return (tileCategory.PackedScenes[(ushort)DungeonBuilder.Random.RandiRange(0, tileCategory.PackedScenes.Count - 1)].CreateOnStage<DungeonTile>(this, DungeonBuilder.ResLoader.DungeonBuilderPreset.StartPosition));
    }
    private DungeonTileCategory GetMin(List<DungeonTileCategory> list)
    {
        DungeonTileCategory minTile = null;
        ushort minPriority = ushort.MaxValue;
        
        foreach (var t in list)
        {
            if (t.DungeonTileCategoryPreset.Priority < minPriority)
            {
                minTile = t;
                minPriority = minTile.DungeonTileCategoryPreset.Priority;
            }
        }

        return minTile;
    }
}