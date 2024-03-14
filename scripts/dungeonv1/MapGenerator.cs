using Godot;
using System.Collections.Generic;
using System.Linq;
using BP.GameConsole;

public class MapGenerator
{
    public MapGrid MapGrid = null;
    public readonly RandomNumberGenerator Random;
    public readonly List<MapWalker> Walkers = new List<MapWalker>();
    
    public ushort CurrentRoomsCount;
    public ushort NumberOfDoneWorkers;

    public ushort NumberOfGeneratedRooms => CurrentRoomsCount;
    
    public ushort TargetRoomsCountPerWalker;
    
    public MapGenerator(ulong seed)
    {
        Random = new RandomNumberGenerator() { Seed = seed };
    }
    private void Update()
    {
        MapGrid.ForEach((xyz, c) =>
        {
            if (c.State)
            {
                Vector2I xy = new Vector2I(xyz.X, xyz.Y);
                List<Vector2I> neighbours = MapGrid.GetNeighborsWith((ushort)xyz.Z, xy, Neighborhood.Manhattan, MapTile.Opened);
                switch (neighbours.Count)
                {
                    case 4: c.Cat = 'c'; break;
                    case 3: c.Cat = 't'; break;
                    case 2: c.Cat = ((xy - neighbours[0]) + (xy - neighbours[1])) == Vector2I.Zero ? 'd' : 'r'; break;
                    default: c.Cat = 's'; break;
                }

                if (c.SubCat == 'm' || c.Id == 0)
                {
                    c.SubCat = 'm';
                }
                else if (c.SubCat == 'f') 
                {
                    c.SubCat = 'f';
                }
                else
                {
                    c.SubCat = 'b';
                }
            }
        });
    }
    public void Start()
    {
        Walkers.Add(new MapWalker(){MapGenerator = this, Position = new Vector2I(MapGrid.Size.X, MapGrid.Size.Y) / 2});

        while (NumberOfDoneWorkers != Walkers.Count)
        {
            // foreach (var walker in _walkers)
            // {
            //     if (walker.IsDone)
            //     {
            //         NumberOfDoneWorkers++;
            //         continue;
            //     }
            //     walker.Dig();
            // }
            for (int i = 0; i < Walkers.Count; i++)
            {
                MapWalker walker = Walkers[i];
                if (walker.IsDone)
                {
                    NumberOfDoneWorkers++;
                    continue;
                }
                walker.Dig();
            }
        }
        GameConsole.Instance.DebugWarning($"Worked: {NumberOfDoneWorkers}");
        Update();
    }
}