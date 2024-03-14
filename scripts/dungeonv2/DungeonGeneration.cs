using System;
using BP.GameConsole;
using Godot;
using Godot.Collections;

public class DungeonGeneration
{
    public RandomNumberGenerator Random = new RandomNumberGenerator();
    public readonly DungeonTier[] DungeonTiers;
    public ushort NumberOfTier { get; }
    public ushort NumberOfRooms { get; }
    public ushort MinDistance { get; }
    public ushort MaxDistance { get; }
    public Vector2I Size { get; }
    private Dictionary<char, ushort> numberOfTypeRooms = new Dictionary<char, ushort>()
    {
        {'m', 1},
        {'f', 3},
    };
    
    public DungeonGeneration(ulong seed, ushort numberOfTiers, ushort distance, Vector2I size)
    {
        if (seed != 0) Random.Seed = seed;

        NumberOfTier = numberOfTiers;
        Size = size;

        DungeonTiers = new DungeonTier[numberOfTiers];
        for (ushort i = 0; i < numberOfTiers; i++)
        {
            DungeonTiers[i] = new DungeonTier(this, i, size);
            foreach (var rule in numberOfTypeRooms)
            {
                for (ushort j = 0; j < rule.Value; j++)
                {
                    DungeonTiers[i].Targets.Add(DungeonTiers[i].Grid.GetRandomPositionBy(1, 3, DungeonTiers[i].Targets).Set(true, 's', rule.Key));
                }
            }
        }
    }
    public void ForEach(Action<Vector3I, DungeonCell> action)
    {
        foreach (DungeonTier tier in DungeonTiers)
        {
            tier.Grid.ForEach((xy, cell) => action(new Vector3I(xy.X, xy.Y, tier.TierId), cell));
        }
    }
}