﻿using System.Collections.Generic;
using System.Diagnostics;
using BP.GameConsole;
using Godot;

public partial class MapBuilder : Node3D
{
    public DungeonGeneration Generation;
    [Export] public ulong Seed { get; private set; }
    [Export] public ushort NumberOfTiers { get; private set; }
    [Export] public ushort Distance { get; private set; }
    [Export] public Vector2I Size { get; private set; }
    [Export] public Vector2I RoomSize { get; private set; }
    [Export] public Vector2I RoomSizeTiles { get; private set; }
    [Export] public Vector3I MapOffset { get; private set; }
    [Export(PropertyHint.Dir)] public string RoomsFolderPath { get; private set; }
    private readonly Dictionary<object, int> _rotates = new Dictionary<object, int>
    {
        { new { id = 1, neightbour = Vector2I.Right }, -180 },
        { new { id = 1, neightbour = Vector2I.Left }, 0 },
        { new { id = 1, neightbour = Vector2I.Up }, -90 },
        { new { id = 1, neightbour = Vector2I.Down },90 },

        { new { id = 2, neightbour = Vector2I.Right }, 180 },
        { new { id = 2, neightbour = Vector2I.Down }, 90 },
        { new { id = 2, neightbour = new Vector2I(-1, -1) }, 180 },
        { new { id = 2, neightbour = new Vector2I(1, 1) }, 0 },
        { new { id = 2, neightbour = new Vector2I(1, -1) }, 90 },
        { new { id = 2, neightbour = new Vector2I(-1, 1) }, -90 },

        { new { id = 3, neightbour = Vector2I.Right }, 90 },
        { new { id = 3, neightbour = Vector2I.Left }, -90 },
        { new { id = 3, neightbour = Vector2I.Up }, 180 },
        { new { id = 3, neightbour = Vector2I.Down },0 },
    };
    private Dictionary<char, Dictionary<char, List<PackedScene>>> RoomsScenes { get; set; } = new Dictionary<char, Dictionary<char, List<PackedScene>>>()
    {
        {'c', new Dictionary<char, List<PackedScene>>() //cross
            {
                {'m', new List<PackedScene>()}, //main
                {'f', new List<PackedScene>()}, //finish
                {'b', new List<PackedScene>()}, //basic
            }
        }, 
        {'t', new Dictionary<char, List<PackedScene>>() //t-pos
            {
                {'m', new List<PackedScene>()}, //main
                {'f', new List<PackedScene>()}, //finish
                {'b', new List<PackedScene>()}, //basic
            }
        }, 
        {'d', new Dictionary<char, List<PackedScene>>() //direct
            {
                {'m', new List<PackedScene>()}, //main
                {'f', new List<PackedScene>()}, //finish
                {'b', new List<PackedScene>()}, //basic
            }
        }, 
        {'r', new Dictionary<char, List<PackedScene>>() //rotate
            {
                {'m', new List<PackedScene>()}, //main
                {'f', new List<PackedScene>()}, //finish
                {'b', new List<PackedScene>()}, //basic
            }
        }, 
        {'s', new Dictionary<char, List<PackedScene>>() //single
            {
                {'m', new List<PackedScene>()}, //main
                {'f', new List<PackedScene>()}, //finish
                {'b', new List<PackedScene>()}, //basic
            }
        }, 
    };

    public override void _Ready()
    {
        Stopwatch workTime = Stopwatch.StartNew();
        
        Generation = new DungeonGeneration(Seed, NumberOfTiers, Distance, Size);
        BuildMap(true);
        
        workTime.Stop();
        GameConsole.Instance.DebugWarning($"Elapsed time: {workTime.Elapsed.TotalMilliseconds} ms");
    }
    public void BuildMap(bool showResult)
    {
        string view = string.Empty;
        Vector3 mapOffset = MapOffset;
        
        LoadRooms();
        Generation.ForEach((xyz, cell) =>
        {
            Vector2I xy = new Vector2I(xyz.X, xyz.Y);
            if (showResult)
            {
                switch (cell.SubCat)
                {
                    case 'b' : view += '\u2592'; break; 
                    case 'f' : view += '\u2588'; break;
                    case 'm' : view += '\u25b2'; break;
                    default: view += '\u2591'; break;
                }
                if (xy.X == Generation.Size.X - 1)
                {
                    GameConsole.Instance.Debug($"{view}");
                    if (xy.Y == Generation.Size.Y - 1)
                    {
                        GameConsole.Instance.DebugLog($"Level number: {xyz.Z}");
                    }
                    view = string.Empty;
                }
            }
            
            if (cell.IsOpen)
            {
                Vector2 roomPosition2d = RoomSize * RoomSizeTiles * xy;
                List<Vector2I> neighbours = Generation.DungeonTiers[xyz.Z].Grid.GetNeighborsBy(xy, Neighborhood.Manhattan, true);
                
                var room = RoomsScenes[cell.Cat][cell.SubCat][0].Instantiate<Node3D>();
                
                if(neighbours.Count > 0  && neighbours.Count < 4) 
                {
                    Vector2I n = Vector2I.Zero;
                
                    foreach (var item in neighbours)
                    {
                        n += item - xy;
                    }
                    if(n == Vector2I.Zero) n = neighbours[0] - xy;
                    room.RotationDegrees = Vector3.Up * _rotates[new { id = neighbours.Count, neightbour = n }];
                }
                mapOffset = Vector3.Up * ((xyz.Z * -4.1f) + MapOffset.Y);
                room.Position = new Vector3(roomPosition2d.X + mapOffset.X, mapOffset.Y, roomPosition2d.Y + mapOffset.Z);

                AddChild(room);
            }
        });
    }
    private void LoadRooms()
    {
        using DirAccess roomsFolder = DirAccess.Open(RoomsFolderPath);

        if (roomsFolder != null)
        {
            roomsFolder.ListDirBegin();

            string filename = roomsFolder.GetNext();

            while (filename != "")
            {
                if (!roomsFolder.CurrentIsDir())
                {
                    string roomPath = roomsFolder.GetCurrentDir().PathJoin(filename);
                    string[] filenameSplit = filename.Split(".")[0].Split("_");
                    char cat = char.Parse(filenameSplit[1]);
                    char subCat = char.Parse(filenameSplit[2]);

                    if (roomPath.Contains(".tscn.remap")) roomPath = roomPath.Replace(".remap", "");
                    RoomsScenes[cat][subCat].Add(ResourceLoader.Load<PackedScene>(roomPath));
                    GameConsole.Instance.DebugLog($"LevelGenerator :: Loaded room at {roomPath}, Filename: {filename}, cat: {cat}, subCat: {subCat}");
                    filename = roomsFolder.GetNext();
                }
            }
            roomsFolder.ListDirEnd();
        }
    }
}