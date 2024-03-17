using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BP.ComponentSystem;
using BP.GameConsole;
using Godot;

public partial class DungeonBuilder : ComponentObject
{
    [Export] private DungeonResourceLoader _resLoader;
    
    private RandomNumberGenerator _random = new ();
    private readonly List<DungeonResourceLoader.DungeonTileProperties> _availableTileScenes = new ();
    private readonly List<DungeonResourceLoader.DungeonTileProperties> _unavailableTileScenes = new ();
    private readonly List<DungeonTile> _validTiles = new ();
    private ushort _currentNumberOfRooms;

    public async void Build()
    {
        Stopwatch workTime = Stopwatch.StartNew();
        GameConsole.Instance.DebugWarning($"Start generation!");
        
        if (_resLoader.LoadTiles(_resLoader.TileScenes) < 1)
        {
            GameConsole.Instance.DebugError($"_resLoader.TileScenes.Count == 0");
            return;
        }

        await Generate();
        
        workTime.Stop();
        GameConsole.Instance.DebugWarning($"Elapsed time: {workTime.Elapsed.TotalMilliseconds} ms, number of generated rooms: {_currentNumberOfRooms}");
    }
    private async Task Generate()
    {
        _random.Seed = _resLoader.DungeonPreset.Seed != 0 ? _resLoader.DungeonPreset.Seed : _random.Seed;
        _availableTileScenes.AddRange(_resLoader.TileScenes.Values);
        _validTiles.Add(await InitTileOrNull());
        
        while (_currentNumberOfRooms < _resLoader.DungeonPreset.NumberOfRooms)
        {
            if(_validTiles.Count < 1) return;
            
            DungeonTile targetRoom = _validTiles[_random.RandiRange(0, _validTiles.Count - 1)];
            
            foreach (Node3D targetConnector in targetRoom.Connectors)
            {
                if(_currentNumberOfRooms >= _resLoader.DungeonPreset.NumberOfRooms || _availableTileScenes.Count < 1) break;
                
                DungeonTile currentRoom = await InitTileOrNull(targetConnector);
                
                if(currentRoom != null) _validTiles.Add(currentRoom);
            }

            _validTiles.Remove(targetRoom);
        }
    }
    private DungeonTile InitTileWithMin(out DungeonResourceLoader.DungeonTileProperties tileProperties)
    {
        return CreateOnStage<DungeonTile>((tileProperties = GetMin()).PackedScenes[(ushort)_random.RandiRange(0, tileProperties.PackedScenes.Count - 1)], _resLoader.DungeonPreset.StartPosition);
    }
    private async Task<DungeonTile> InitTileOrNull(Node3D targetSnap = null)
    {
        DungeonTile tile = InitTileWithMin(out var tileProperties);

        if (targetSnap != null)
        {
            Node3D currentConnector = tile.Connectors[(ushort)_random.RandiRange(0, tile.Connectors.Count - 1)];
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
        
        if (tileProperties.CurrentNumberOfTilesPerTier >= tileProperties.NumberOfTilesPerTier - 1)
        {
            _availableTileScenes.Remove(tileProperties);
            _unavailableTileScenes.Add(tileProperties);
        }
                    
        tileProperties.CurrentNumberOfTilesPerTier++;
        _currentNumberOfRooms++;
        
        return tile;
    }

    private DungeonResourceLoader.DungeonTileProperties GetMin()
    {
        DungeonResourceLoader.DungeonTileProperties minTile = _availableTileScenes[0];
        ushort minPriority = minTile.Priority;

        for (int i = 1; i < _availableTileScenes.Count; i++)
        {
            if (_availableTileScenes[i].Priority < minPriority)
            {
                minTile = _availableTileScenes[i];
                minPriority = minTile.Priority;
            }
        }

        return minTile;
    }
    public T CreateOnStage<T>(PackedScene scene) where T : Node
    {
        T obj = scene.Instantiate<T>();
        AddChild(obj);
        return obj;
    }
    public T CreateOnStage<T>(PackedScene scene, Vector3 position) where T : Node3D
    {
        T obj = scene.Instantiate<T>();
        obj.Position = position;
        AddChild(obj);
        return obj;
    }
}