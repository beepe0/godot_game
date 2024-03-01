using BP.GameConsole;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class DungeonBuilder : Node3D
{
    public static DungeonBuilder Instance { get; private set; }

    [Export(PropertyHint.Dir)] public string RoomsFolderPath { get; private set; }
    [Export] public ushort TargetRoomsCount { get; private set; }
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
        Generate(0);
    }
    public void Generate(ulong seed)
    {
        Grid<bool> mapGrid = new Grid<bool>(LevelSize, false);
        Generator mapGenerator = new Generator() { Grid = mapGrid, TargetRoomsCount = TargetRoomsCount };
        string view = string.Empty;
        ushort currentRoom = 0;
        
        if (seed != 0) Game.Instance.Rng.Seed = seed;
        GameConsole.Instance.DebugLogCallDeferrd($"Random Seed: {Game.Instance.Rng.Seed}");
        
        ushort mainRoomId = (ushort)Game.Instance.Rng.RandiRange(TargetRoomsCount/2, TargetRoomsCount-1);
        ushort finishRoomId = (ushort)Game.Instance.Rng.RandiRange(0, TargetRoomsCount/2);

        LoadRooms();

        mapGenerator.Generate();

        for (int y = 0; y < mapGrid.Size.Y; y++)
        {
            for (int x = 0; x < mapGrid.Size.X; x++)
            {
                if (mapGrid[x, y])
                {
                    Node3D room;
                    Vector2 roomPosition2d = TileSize * RoomSizeTiles * new Vector2I(x, y);
                    List<Vector2I> neighbours = mapGrid.GetNeighborsWith(new Vector2I(x, y), Neighborhood.Manhattan, mapGrid[x, y]);
                    char cat = ' ';
                    char subCat = ' ';
                    
                    switch (neighbours.Count)
                    {
                        case 4: cat = 'c'; break;
                        case 3: cat = 't'; break;
                        case 2: cat = ((new Vector2I(x, y) - neighbours[0]) + (new Vector2I(x, y) - neighbours[1])) == Vector2I.Zero ? 'd' : 'r'; break;
                        case 1: cat = 's'; break;
                    }

                    if (mainRoomId == currentRoom)
                    {
                        subCat = 'm';
                        view += "M";
                    }
                    // else if (finishRoomId == currentRoom)
                    // {
                    //     //subCat = 'm';
                    //     view += "F";
                    // }
                    else
                    {
                        subCat = 'b';
                        view += "▓";
                    }
                    room = RoomsScenes[cat][subCat][Game.Instance.Rng.RandiRange(0, (RoomsScenes[cat][subCat].Count - 1) < 0 ? 0 : RoomsScenes[cat][subCat].Count - 1)].Instantiate<Node3D>();
                    
                    if(neighbours.Count < 4) 
                    {
                        Vector2I n = Vector2I.Zero;
                    
                        foreach (var item in neighbours)
                        {
                            n += item - new Vector2I(x, y);
                        }
                        if(n == Vector2I.Zero) n = neighbours[0] - new Vector2I(x, y);
                        room.RotationDegrees = Vector3.Up * _rotates[new { id = neighbours.Count, neightbour = n }];
                    }
                    room.Position = new Vector3(roomPosition2d.X + MapOffset.X, MapOffset.Y, roomPosition2d.Y + MapOffset.Z);
                    AddChild(room);
                    currentRoom++;
                }
                else view += "░";
            }
            GameConsole.Instance.DebugCallDeferrd(view);
            view = string.Empty;
        }
        GameConsole.Instance.DebugLogCallDeferrd($"currentRoom: {currentRoom}, mainRoomId: {mainRoomId}, finishRoomId: {finishRoomId}");
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
