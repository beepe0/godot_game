using System.Collections.Generic;
using Godot;

public class DungeonTier
{
    public readonly ushort TierId;
    public readonly List<DungeonCell> Targets = new List<DungeonCell>();
    
    public readonly DungeonGeneration Generation;
    public readonly DungeonGrid Grid;
    private readonly DungeonWalker[] _dungeonWalkers = new[] { new DungeonWalker() };
    public bool IsDone;

    public DungeonTier(DungeonGeneration generation, ushort tierId, Vector2I size)
    {
        Generation = generation;
        TierId = tierId;
        Grid = new DungeonGrid(this, new Vector2I(size.X, size.Y));
    }
    public void Generate()
    {
        while (!IsDone)
        {
            foreach (DungeonWalker walker in _dungeonWalkers)
            {
                Grid[Grid.GetBetterRandomPosition(Grid[walker.Position], Targets)].Set(true, 's', 'b');
            }
        }
    }
}