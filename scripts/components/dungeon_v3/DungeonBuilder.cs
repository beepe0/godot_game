using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BP.ComponentSystem;
using BP.DebugGizmos;
using BP.GameConsole;
using Godot;

[GlobalClass]
public partial class DungeonBuilder : ComponentObject
{
    [Export] public DungeonBuilderPreset DungeonBuilderPreset;
    [Export] public DungeonResourceLoader ResLoader;
    [Export] public DungeonCulling DungeonCulling;
    
    public RandomNumberGenerator Random = new();
    public ushort CurrentNumberOfTiers;
    public ushort CurrentNumberOfTiles;
    
    public DungeonTier[] DungeonTiers;

    public async void Build()
    {
        Stopwatch workTime = Stopwatch.StartNew();

        DungeonTiers = new DungeonTier[DungeonBuilderPreset.NumberOfTiers];

        for (ushort i = 0; i < DungeonBuilderPreset.NumberOfTiers; i++)
        {
            DungeonTiers[i] = new DungeonTier(){Name = $"Tier_{i}", DungeonBuilder = this};
            AddChild(DungeonTiers[i]);
        }

        foreach (var preset in DungeonBuilderPreset.CategoryScenes)
        {
            for (ushort i = 0; i < preset.ValidNeighbours.Length; i++)
            {
                preset.AvailableTileScenes.Add(DungeonBuilderPreset.CategoryScenes.FirstOrDefault(e => e.TilesCategory == preset.ValidNeighbours[i]));
            }
        }
        
        GameConsole.Instance.DebugWarning($"Start generation!");

        ResLoader.LoadResources();
        
        await Generate();
        
        DungeonCulling.Begin();
        workTime.Stop();
        
        GameConsole.Instance.DebugWarning($"Elapsed time: {workTime.Elapsed.TotalMilliseconds} ms, number of generated tiles: {CurrentNumberOfTiles}");
    }
    private async Task Generate()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        Random.Seed = DungeonBuilderPreset.Seed != 0 ? DungeonBuilderPreset.Seed : Random.Seed;
        GameConsole.Instance.DebugLog($"Random Seed: {Random.Seed}");
        
        CreateRoot();

        while (CurrentNumberOfTiers < DungeonBuilderPreset.NumberOfTiers)
        {
            DungeonTile targetRoom = DungeonTiers[CurrentNumberOfTiers].ValidTiles[Random.RandiRange(0, DungeonTiers[CurrentNumberOfTiers].ValidTiles.Count - 1)];
            await DungeonTiers[CurrentNumberOfTiers].AddTiles(targetRoom);
            
            if(DungeonTiers[CurrentNumberOfTiers].ValidTiles.Count < 1)
            {
                GameConsole.Instance.Debug(CurrentNumberOfTiers);
                if(DungeonTiers[CurrentNumberOfTiers].ValidTilesFrom.Count < 1) break;
                
                CurrentNumberOfTiers++;
                
                if (CurrentNumberOfTiers >= DungeonBuilderPreset.NumberOfTiers) break;
                
                DungeonTiers[CurrentNumberOfTiers].ValidTiles.AddRange(DungeonTiers[CurrentNumberOfTiers - 1].ValidTilesFrom);

                foreach (var preset in DungeonBuilderPreset.CategoryScenes)
                {
                    preset.ResetTempProperties();
                }
            }
        }
    }
    private void CreateRoot()
    {
        DungeonTileCategoryPreset minTile = null;
        ushort minPriority = ushort.MaxValue;

        foreach (DungeonTileCategoryPreset cat in DungeonBuilderPreset.CategoryScenes)
        {
            if (cat.Priority < minPriority)
            {
                minTile = cat;
                minPriority = cat.Priority;
            }
        }
        
        DungeonTile tile = minTile.PackedScenes[(ushort)Random.RandiRange(0, minTile.PackedScenes.Count - 1)].CreateOnStage<DungeonTile>(DungeonTiers[CurrentNumberOfTiers], DungeonBuilderPreset.StartPosition);

        DungeonTiers[CurrentNumberOfTiers].ValidTiles.Add(tile);
        DungeonTiers[CurrentNumberOfTiers].Tiles.Add(tile);
        
        minTile!.CurrentNumberOfTilesPerTier++;
        CurrentNumberOfTiles++;
    }
}