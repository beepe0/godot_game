using System;
using System.Collections.Generic;
using System.Linq;
using BP.GameConsole;
using Godot;

public class DungeonGrid
{
    public readonly DungeonTier Tier;
    private readonly DungeonCell[,] _cells = null;
    public Vector2I Size { get; private set; }

    public int Width => Size.X;
    public int Height => Size.Y;
    
    public DungeonGrid(DungeonTier tier, Vector2I size)
    {
        Tier = tier;
        Size = size;
        _cells = new DungeonCell[Width, Height];
        
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _cells[x, y] = new DungeonCell() {IsOpen = false, Position = new Vector2I(x, y)};
            }
        }
    }
    private bool IsInBounds(Vector2I position)
    {
        return position.X > (-1) && position.X < Width && position.Y > (-1) && position.Y < Height;
    }
    public List<Vector2I> GetNeighborsBy(DungeonCell cell, Vector2I[] neighborhood, bool state)
    {
        return neighborhood.Select(neighbor => neighbor + cell.Position).Where(neighbor => IsInBounds(neighbor) && _cells[neighbor.X, neighbor.Y].IsOpen == state).ToList();
    }
    public List<Vector2I> GetNeighborsBy(Vector2I cell, Vector2I[] neighborhood, bool state)
    {
        return neighborhood.Select(neighbor => neighbor + cell).Where(neighbor => IsInBounds(neighbor) && _cells[neighbor.X, neighbor.Y].IsOpen == state).ToList();
    }
    public Vector2I GetBetterRandomPosition(DungeonCell cell, List<DungeonCell> targets)
    {
        DungeonCell target = targets[0];
        float minWeight = GetLength(cell.Position, target.Position);
        float averageWeight = 0;

        for (ushort i = 1; i < targets.Count; i++)
        {
            float weight = GetLength(cell.Position, targets[i].Position);
            if (weight < minWeight)
            {
                target = targets[i];
                minWeight = weight;
            }
        }
        List<Vector2I> closedNeighbors = GetNeighborsBy(cell, Neighborhood.Manhattan, false);
        closedNeighbors.ForEach(n => averageWeight += GetLength(target.Position, n));
        averageWeight /= closedNeighbors.Count;
        averageWeight += 0.1f;

        Vector2I[] validNeighbors = closedNeighbors.Where(e =>
        {
            float l = GetLength(target.Position, e);
            bool b = l < averageWeight;
            return b;
        }).ToArray();
        
        // if (validNeighbors.Length < 1)
        // {
        //     Tier.IsDone = true;
        //     return Vector2I.Zero;
        // }
        // return validNeighbors[Tier.Generation.Random.RandiRange(0, validNeighbors.Length - 1)];
        if (validNeighbors.Length < 1)
        {
            Tier.IsDone = true;
            return Vector2I.Zero;
        }
        return validNeighbors[0];
    }

    public DungeonCell GetRandomPositionBy(float min, float max, List<DungeonCell> around)
    {
        Vector2I result = GetRandomPosition();
        if (around.Count > 0)
        {
            bool ok = false;
            while (!ok)
            {
                foreach (var cell in around)
                {
                    float mag = (result - cell.Position).Abs().Length();
                    if (mag > min && mag < max )
                    {
                        ok = true;
                    }
                    else
                    {
                        ok = false;
                        result = GetRandomPosition();
                        break;
                    }
                }
            }
        }

        return this[result];
    }
    public Vector2I GetRandomPosition()
    {
        return new Vector2I(Tier.Generation.Random.RandiRange(0, Size.X - 1), Tier.Generation.Random.RandiRange(0, Size.Y - 1));
    }
    public float GetLength(Vector2I a, Vector2I b)
    {
        return (a - b).Abs().Length();
    }

    public void ForEach(Action<Vector2I, DungeonCell> action)
    {
        for (ushort y = 0; y < Size.Y; y++)
        {
            for (ushort x = 0; x < Size.X; x++)
            {
                action(new Vector2I(x, y),  _cells[x, y]);
            }
        }
    }
    public DungeonCell this[Vector2I position]
    {
        get => _cells[position.X, position.Y];
        set => _cells[position.X, position.Y] = value;
    }
    public DungeonCell this[int x, int y]
    {
        get => _cells[x, y];
        set => _cells[x, y] = value;
    }
}

public class DungeonCell
{
    public Vector2I Position;
    
    public bool IsOpen;
    public char Cat;
    public char SubCat;

    public DungeonCell(){}

    public DungeonCell Set(bool isOpen, char cat, char subCat)
    {
        IsOpen = isOpen;
        Cat = cat;
        SubCat = subCat;
        return this;
    }
}
public static class Neighborhood
{
    public static readonly Vector2I[] Manhattan = new Vector2I[] { Vector2I.Right, Vector2I.Left, Vector2I.Down, Vector2I.Up };
    public static readonly Vector2I[] Moore = Manhattan.Concat(new Vector2I[] { new Vector2I(1, 1), new Vector2I(-1, 1), new Vector2I(1, -1), new Vector2I(-1, -1) }).ToArray();
}