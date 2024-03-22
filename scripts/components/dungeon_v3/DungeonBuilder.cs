using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BP.ComponentSystem;
using BP.GameConsole;
using Godot;

public partial class DungeonBuilder : ComponentObject
{
    [Export] public DungeonResourceLoader ResLoader;
    
    public RandomNumberGenerator Random = new ();
    public readonly Dictionary<string, DungeonTileCategory> TileScenes = new ();

    private readonly List<DungeonTile> _validTiles = new ();
    public ushort CurrentNumberOfRooms;

    public async void Build()
    {
        Stopwatch workTime = Stopwatch.StartNew();
        GameConsole.Instance.DebugWarning($"Start generation!");
        
        if (ResLoader.LoadResources(TileScenes) < 1)
        {
            GameConsole.Instance.DebugError($"_resLoader.TileScenes.Count == 0");
            return;
        }

        await ToSignal(GetTree().CreateTimer(1), "timeout");
        await Generate();
        
        workTime.Stop();
        GameConsole.Instance.DebugWarning($"Elapsed time: {workTime.Elapsed.TotalMilliseconds} ms, number of generated rooms: {CurrentNumberOfRooms}");
    }
    private async Task Generate()
    {
        Random.Seed = ResLoader.DungeonBuilderPreset.Seed != 0 ? ResLoader.DungeonBuilderPreset.Seed : Random.Seed;
        GameConsole.Instance.DebugLog($"Random Seed: {Random.Seed}");

        _validTiles.Add(CreateRoot());
        
        while (CurrentNumberOfRooms < ResLoader.DungeonBuilderPreset.NumberOfRooms)
        {
            if(_validTiles.Count < 1) return;
            
            DungeonTile targetRoom = _validTiles[Random.RandiRange(0, _validTiles.Count - 1)];
            _validTiles.AddRange(await TileScenes[targetRoom.Category].Execute(targetRoom));

            _validTiles.Remove(targetRoom);
        }
    }

    private DungeonTile CreateRoot()
    {
        DungeonTileCategory minTile = null;
        ushort minPriority = ushort.MaxValue;

        foreach (DungeonTileCategory cat in TileScenes.Values)
        {
            if (cat.DungeonTileCategoryPreset.Priority < minPriority)
            {
                minTile = cat;
                minPriority = minTile.DungeonTileCategoryPreset.Priority;
            }
        }
        minTile.CurrentNumberOfTilesPerTier++;
        CurrentNumberOfRooms++;
        
        return minTile.PackedScenes[(ushort)Random.RandiRange(0, minTile.PackedScenes.Count - 1)].CreateOnStage<DungeonTile>(this, ResLoader.DungeonBuilderPreset.StartPosition);
    }
}