using Godot;
using System.Collections.Generic;
using System.Linq;

public class MapGrid
{
    private readonly MapTile[,] _cells = null;
    public Vector2I Size { get; private set; }

    public int Width => Size.X;
    public int Height => Size.Y;

    public MapGrid(Vector2I size, MapTile defaultCellValue)
    {
        Size = size;
        _cells = new MapTile[size.X, size.Y];
        Fill(defaultCellValue.State);
    }
    public bool IsInBounds(Vector2I position)
    {
        return position.X > (-1) && position.X < Width && position.Y > (-1) && position.Y < Height;
    }
    public void Fill(bool state)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _cells[x, y] = new MapTile(state);
            }
        }
    }
    public List<Vector2I> GetNeighbors(Vector2I position, Vector2I[] neighborhood)
    {
        return neighborhood.Select(neighbor => neighbor + position).Where(neighbor => IsInBounds(neighbor)).ToList();
    }
    public List<Vector2I> GetNeighborsWith(Vector2I position, Vector2I[] neighborhood, MapTile cell)
    {
        return neighborhood.Select(neighbor => neighbor + position).Where(neighbor => IsInBounds(neighbor) && _cells[neighbor.X, neighbor.Y].State == cell.State).ToList();
    }
    public MapTile this[Vector2I position]
    {
        get => _cells[position.X, position.Y];
        set => _cells[position.X, position.Y] = value;
    }
    public MapTile this[int x, int y]
    {
        get => _cells[x, y];
        set => _cells[x, y] = value;
    }
}
public class MapTile
{
    public static readonly MapTile Opened = new MapTile(true);
    public static readonly MapTile Closed = new MapTile(false);

    public bool State;
    public char Cat;
    public char SubCat;

    public MapTile(bool state)
    {
        State = state;
    }
}