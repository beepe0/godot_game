using BP.GameConsole;
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using static Godot.TextServer;

public partial class LevelGenerator : Node3D
{
    [Export(PropertyHint.Dir)]
    public string RoomsFolderPath { get; private set; }

    public Dictionary<char, List<PackedScene>> RoomsScenes { get; private set; } = new Dictionary<char, List<PackedScene>>()
    {
        {'c', new List<PackedScene>()},
        {'t', new List<PackedScene>()},
        {'d', new List<PackedScene>()},
        {'r', new List<PackedScene>()},
        {'e', new List<PackedScene>()},
    };

    [Export]
    public ushort TargetRoomsCount { get; private set; }

    [Export]
    public Vector2I LevelSize { get; private set; }

    [Export]
    public Vector2 TileSize {  get; private set; }

    [Export]
    public Vector2I RoomSizeTiles { get; private set; }

    private Dictionary<object, int> _rotates = new Dictionary<object, int>
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

    private string _view;
    public override void _Ready()
    {
        Grid<bool> grid = new Grid<bool>(LevelSize, false);
        Generator generator = new Generator() { Grid = grid, TargetRoomsCount = TargetRoomsCount };
        Node3D room = null;
        Vector2 position2D;

        LoadRooms();

        Game.Instance.Rng.Seed = 1;
        GameConsole.Instance.DebugWarningCallDeferrd($"Random Seed: {Game.Instance.Rng.Seed}");

        generator.Generate();
        for (int y = 0; y < grid.Size.Y; y++)
        {
            for (int x = 0; x < grid.Size.X; x++)
            {
                _view += grid[x, y] ? "▓" : "░";
                if (grid[x, y])
                {
                    List<Vector2I> neightbours = grid.GetNeighborsWith(new Vector2I(x, y), Neighborhood.Manhattan, grid[x, y]);
                    char cat = ' ';

                    switch (neightbours.Count)
                    {
                        case 4: cat = 'c'; break;
                        case 3: cat = 't'; break;
                        case 2: cat = ((new Vector2I(x, y) - neightbours[0]) + (new Vector2I(x, y) - neightbours[1])) == Vector2I.Zero ? 'd' : 'r'; break;
                        case 1: cat = 'e'; break;
                    }
                    room = RoomsScenes[cat][Game.Instance.Rng.RandiRange(0, RoomsScenes[cat].Count - 1)].Instantiate<Node3D>();

                    position2D = TileSize * RoomSizeTiles * new Vector2I(x, y);
                    room.Position = new Vector3(position2D.X, 0, position2D.Y);

                    if(neightbours.Count < 4) 
                    {
                        Vector2I n = Vector2I.Zero;

                        foreach (var item in neightbours)
                        {
                            n += item - new Vector2I(x, y);
                        }
                        if(n == Vector2I.Zero) n = neightbours[0] - new Vector2I(x, y);
                        room.RotationDegrees = Vector3.Up * _rotates[new { id = neightbours.Count, neightbour = n }];
                    }
                    AddChild(room);
                }
            }
            GameConsole.Instance.DebugCallDeferrd(_view);
            _view = string.Empty;
        }
    }
    private void LoadRooms()
    {
        List<PackedScene> loadedRooms = new List<PackedScene>();

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

                    RoomsScenes[cat].Add(ResourceLoader.Load<PackedScene>(roomPath));
                    GameConsole.Instance.DebugLog($"LevelGenerator :: Loaded room at {roomPath}, Filename: {filename}");
                    filename = roomsFolder.GetNext();
                }
            }
            roomsFolder.ListDirEnd();
        }
    }

}
