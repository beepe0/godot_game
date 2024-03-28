using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BP.ComponentSystem;
using BP.GameConsole;
using Godot;

public partial class DungeonBuilder : ComponentObject
{
    [Export] public DungeonResourceLoader ResLoader;
    
    public RandomNumberGenerator Random = new ();
    public readonly Dictionary<string, DungeonTileCategory> TileScenes = new ();

    public readonly List<DungeonTile> ValidTiles = new ();
    public readonly List<DungeonTile> ValidTilesFrom = new ();
    public ushort CurrentNumberOfTiers;
    public ushort CurrentNumberOfTiles;

    public async void Build()
    {
        Stopwatch workTime = Stopwatch.StartNew();
        GameConsole.Instance.DebugWarning($"Start generation!");
        
        if (ResLoader.LoadResources(TileScenes) < 1)
        {
            GameConsole.Instance.DebugError($"_resLoader.TileScenes.Count == 0");
            return;
        }
        await Generate();
        
        workTime.Stop();
        
        foreach (var tileCat in TileScenes.Values) 
            GameConsole.Instance.DebugWarning($"Number of generated {tileCat.DungeonTileCategoryPreset.TilesCategory} tiles: {tileCat.CurrentNumberOfTilesPerTier}");
        
        GameConsole.Instance.DebugWarning($"Elapsed time: {workTime.Elapsed.TotalMilliseconds} ms, number of generated tiles: {CurrentNumberOfTiles}");
    }
    private async Task Generate()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        Random.Seed = ResLoader.DungeonBuilderPreset.Seed != 0 ? ResLoader.DungeonBuilderPreset.Seed : Random.Seed;
        GameConsole.Instance.DebugLog($"Random Seed: {Random.Seed}");

        ValidTiles.Add(CreateRoot());
        
        while (CurrentNumberOfTiers < ResLoader.DungeonBuilderPreset.NumberOfTiers)
        {
            if(ValidTiles.Count < 1)
            {
                if (CurrentNumberOfTiers >= ResLoader.DungeonBuilderPreset.NumberOfTiers || ValidTilesFrom.Count < 1) break;
                GameConsole.Instance.DebugLog($"CurrentNumberOfTiers: {CurrentNumberOfTiers}, ValidTiles.Count: {ValidTiles.Count}, ValidTilesFrom.Count: {ValidTilesFrom.Count}");

                ValidTiles.Clear();
                ValidTiles.AddRange(ValidTilesFrom);
                ValidTilesFrom.Clear();
                
                foreach (var cat in TileScenes.Values)
                {
                    cat.Reset();
                }

                CurrentNumberOfTiers++;
            }

            DungeonTile targetRoom = ValidTiles[Random.RandiRange(0, ValidTiles.Count - 1)];
            ValidTiles.AddRange(await TileScenes[targetRoom.Category].Execute(targetRoom, true));
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
        minTile!.CurrentNumberOfTilesPerTier++;
        CurrentNumberOfTiles++;
        
        return minTile.PackedScenes[(ushort)Random.RandiRange(0, minTile.PackedScenes.Count - 1)].CreateOnStage<DungeonTile>(this, ResLoader.DungeonBuilderPreset.StartPosition);
    }
}