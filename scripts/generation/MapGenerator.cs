using System;
using Godot;
using System.Collections.Generic;
using System.Linq;

public class MapGenerator
{
    public readonly RandomNumberGenerator Random;
    public readonly List<MapWalker> Walkers = new List<MapWalker>() { new MapWalker() };
    
    public MapGrid MapGrid = null;
    private readonly List<MapWalker> _walkersToAdd = new List<MapWalker>();
    private ushort _currentRoomsCount;
    public ushort NumberOfGeneratedRooms => _currentRoomsCount;

    public ushort MainRoomId;
    public ushort FinishRoomId;
    public ushort TargetRoomsCount;

    public MapGenerator(ulong seed)
    {
        Random = new RandomNumberGenerator() { Seed = seed };
    }
    public void Update()
    {
        foreach (MapWalker walker in Walkers)
        {
            if (_currentRoomsCount >= TargetRoomsCount)
            {
                break;
            }

            if (MapGrid[walker.Position].State == false)
            {
                MapGrid[walker.Position].State = true;
                _currentRoomsCount++;
            }
            
            List<Vector2I> validNeighbours = new List<Vector2I>();
            List<Vector2I> tempNeighbours = MapGrid.GetNeighborsWith(walker.Position, Neighborhood.Manhattan, MapTile.Closed);
            
            foreach (Vector2I neighbor in tempNeighbours)
            {
                if(MapGrid.GetNeighborsWith(neighbor, Neighborhood.Moore, MapTile.Opened).Count < 4)
                {
                    validNeighbours.Add(neighbor);
                }
            }

            if (validNeighbours.Count > 0)
            {
                walker.MoveHistory.Push(walker.Position);

                Vector2I targetPosition = validNeighbours[Random.RandiRange(0, validNeighbours.Count - 1)];

                if (Random.Randf() <= 0.05f)
                {
                    MapWalker newMapWalker = new MapWalker
                    {
                        Position = targetPosition,
                        MoveHistory = new Stack<Vector2I>(walker.MoveHistory)
                    };

                    _walkersToAdd.Add(newMapWalker);
                }
                else
                {
                    walker.Position = targetPosition;
                }
            }
            else if(walker.MoveHistory.Count > 0)
            {
                walker.Position = walker.MoveHistory.Pop();
            }
        }

        ForEach((yx, c) =>
        {
            if (c.State)
            {
                List<Vector2I> neighbours = MapGrid.GetNeighborsWith(yx, Neighborhood.Manhattan, MapTile.Opened);
                switch (neighbours.Count)
                {
                    case 4: c.Cat = 'c'; break;
                    case 3: c.Cat = 't'; break;
                    case 2: c.Cat = ((yx - neighbours[0]) + (yx - neighbours[1])) == Vector2I.Zero ? 'd' : 'r'; break;
                    case 1: c.Cat = 's'; break;
                }

                switch (_currentRoomsCount)
                {
                    
                }
            }
        });
        Walkers.AddRange(_walkersToAdd);
        _walkersToAdd.Clear();
    }
    public void Generate()
    {
        MainRoomId = (ushort)Random.RandiRange(TargetRoomsCount / 2, TargetRoomsCount - 1);
        FinishRoomId = (ushort)Random.RandiRange(0, TargetRoomsCount / 2);

        foreach (MapWalker walker in Walkers)
        {
            walker.Position = MapGrid.Size / 2;
        }
        while (_currentRoomsCount < TargetRoomsCount) Update();
    }
    public void ForEach(Action<Vector2I, MapTile> action)
    {
        for (ushort y = 0; y < MapGrid.Size.Y; y++)
        {
            for (ushort x = 0; x < MapGrid.Size.X; x++)
            {
                action(new Vector2I(x, y), MapGrid[x, y]);
            }
        }
    }
}
public static class Neighborhood
{
    public static readonly Vector2I[] Manhattan = new Vector2I[] { Vector2I.Right, Vector2I.Left, Vector2I.Down, Vector2I.Up };
    public static readonly Vector2I[] Moore = Manhattan.Concat(new Vector2I[] { new Vector2I(1, 1), new Vector2I(-1, 1), new Vector2I(1, -1), new Vector2I(-1, -1) }).ToArray();
}