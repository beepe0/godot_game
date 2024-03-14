using System;
using Godot;
using System.Collections.Generic;
using System.Linq;

public class MapGrid
{
    //public readonly List<List<MapTile>> OpenedCells = new List<List<MapTile>>();
    private readonly MapTile[,,] _cells = null;
    public Vector3I Size { get; private set; }

    public int Width => Size.X;
    public int Height => Size.Y;
    public int Depth => Size.Z;

    public MapGrid(Vector3I size, MapTile defaultCellValue)
    {
        Size = size;
        _cells = new MapTile[Width, Height, Depth];
        // for (ushort i = 0; i < Depth; i++)
        // {
        //     OpenedCells.Add(new List<MapTile>());
        // }
        Fill(defaultCellValue.State);
    }
    public bool IsInBounds(Vector2I position)
    {
        return position.X > (-1) && position.X < Width && position.Y > (-1) && position.Y < Height;
    }
    public void Fill(bool state)
    {
        for (int z = 0; z < Depth; z++)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _cells[x, y, z] = new MapTile(state);
                }
            }
        }
    }
    public List<Vector2I> GetNeighbors(Vector2I position, Vector2I[] neighborhood)
    {
        return neighborhood.Select(neighbor => neighbor + position).Where(neighbor => IsInBounds(neighbor)).ToList();
    }
    public List<Vector2I> GetNeighborsWith(ushort level, Vector2I position, Vector2I[] neighborhood, MapTile cell)
    {
        return neighborhood.Select(neighbor => neighbor + position).Where(neighbor => IsInBounds(neighbor) && _cells[neighbor.X, neighbor.Y, level].State == cell.State).ToList();
    }

    public void ForEach(Action<Vector3I, MapTile> action)
    {
        for (ushort z = 0; z < Size.Z; z++)
        {
            for (ushort y = 0; y < Size.Y; y++)
            {
                for (ushort x = 0; x < Size.X; x++)
                {
                    action(new Vector3I(x, y, z),  _cells[x, y, z]);
                }
            }
        }
    }
    public MapTile this[Vector2I position, ushort level]
    {
        get => _cells[position.X, position.Y, level];
        set => _cells[position.X, position.Y, level] = value;
    }
    public MapTile this[int x, int y, ushort level]
    {
        get => _cells[x, y, level];
        set => _cells[x, y, level] = value;
    }
}
public class MapTile
{
    public static readonly MapTile Opened = new MapTile(true);
    public static readonly MapTile Closed = new MapTile(false);
    public MapWalker Who;

    public bool State;
    public ushort Id;
    public char Cat;
    public char SubCat;

    public MapTile(bool state)
    {
        State = state;
    }
    public MapTile Set(bool state, ushort id, char subcat)
    {
        State = state;
        Id = id;
        SubCat = subcat;
        return this;
    }
}