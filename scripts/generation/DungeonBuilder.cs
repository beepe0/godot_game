using BP.GameConsole;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DungeonBuilder : Node3D
{
    public static DungeonBuilder Instance { get; private set; }

    [Export(PropertyHint.Dir)] public string RoomsFolderPath { get; private set; }
    [Export] public ushort TargetRooms { get; private set; }
    [Export] public ushort TargetLevels { get; private set; }
    [Export] public Vector2I LevelSize { get; private set; }
    [Export] public Vector2 TileSize { get; private set; }
    [Export] public Vector3 MapOffset { get; private set; }
    [Export] public Vector2I RoomSizeTiles { get; private set; }

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

    public override void _EnterTree()
    {
        Instance = this;
    }
    public override void _Ready()
    {
        LoadRooms();
        Generate(1, TargetRooms, TargetLevels, true);
    }
    public void Generate(ulong seed, ushort targetRooms, ushort targetLevels, bool showResult)
    {
        GameConsole.Instance.DebugLogCallDeferrd($"Random Seed: {seed}");
        
        MapGenerator mapGenerator = new MapGenerator(seed) { MapGrid = new MapGrid(LevelSize, MapTile.Closed), TargetRoomsCount = targetRooms };
        string viewOriginal = string.Empty;
        string view = string.Empty;

        mapGenerator.Generate();
        mapGenerator.ForEach((yx, c) =>
        {
            if (showResult)
            {
                viewOriginal += c.Cat;
                view += c.State ? '\u25d9' : '\u25cb';
                if (yx.X == mapGenerator.MapGrid.Size.X - 1)
                {
                    GameConsole.Instance.Debug(viewOriginal);
                    GameConsole.Instance.DebugCallDeferrd(view);
                    viewOriginal = string.Empty;
                    view = string.Empty;
                }
            }
            
            if (c.State)
            {
                Node3D room;
                Vector2 roomPosition2d = TileSize * RoomSizeTiles * yx;
                List<Vector2I> neighbours = mapGenerator.MapGrid.GetNeighborsWith(yx, Neighborhood.Manhattan, mapGenerator.MapGrid[yx]);
                
                room = RoomsScenes[c.Cat][c.SubCat][mapGenerator.Random.RandiRange(0, (RoomsScenes[c.Cat][c.SubCat].Count - 1) < 0 ? 0 : RoomsScenes[c.Cat][c.SubCat].Count - 1)].Instantiate<Node3D>();
                
                if(neighbours.Count < 4) 
                {
                    Vector2I n = Vector2I.Zero;
                
                    foreach (var item in neighbours)
                    {
                        n += item - yx;
                    }
                    if(n == Vector2I.Zero) n = neighbours[0] - yx;
                    room.RotationDegrees = Vector3.Up * _rotates[new { id = neighbours.Count, neightbour = n }];
                }
                room.Position = new Vector3(roomPosition2d.X + MapOffset.X, MapOffset.Y, roomPosition2d.Y + MapOffset.Z);
                AddChild(room);
            }
        });
        GameConsole.Instance.DebugLogCallDeferrd($"numberOfRooms: {mapGenerator.NumberOfGeneratedRooms}, mainRoomId: {mapGenerator.MainRoomId}, finishRoomId: {mapGenerator.FinishRoomId}");
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
