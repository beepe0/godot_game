using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Grid<TCell> where TCell : struct
{
    private readonly TCell[,] _cells = null;

    public Vector2I Size { get; private set; } = Vector2I.Zero;

    public int Width => Size.X;
    public int Height => Size.Y;

    public Grid(Vector2I size, TCell defaultCellValue)
    {
        Size = size;

        _cells = new TCell[size.X, size.Y];

        Fill(defaultCellValue);
    }
    public bool IsInBounds(Vector2I position)
    {
        return position.X > (-1) && position.X < Width && position.Y > (-1) && position.Y < Height;
    }
    public void Fill(TCell with)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _cells[x, y] = with;
            }
        }
    }
    public List<Vector2I> GetNeighbors(Vector2I position, Vector2I[] neighborhood)
    {
        return neighborhood.Select(neighbor => neighbor + position).Where(neighbor => IsInBounds(neighbor)).ToList();
    }
    public List<Vector2I> GetNeighborsWith(Vector2I position, Vector2I[] neighborhood, TCell cell)
    {
        return neighborhood.Select(neighbor => neighbor + position).Where(neighbor => IsInBounds(neighbor) && _cells[neighbor.X, neighbor.Y].Equals(cell)).ToList();
    }
    public TCell this[Vector2I position]
    {
        get => _cells[position.X, position.Y];
        set => _cells[position.X, position.Y] = value;
    }
    public TCell this[int x, int y]
    {
        get => _cells[x, y];
        set => _cells[x, y] = value;
    }
}