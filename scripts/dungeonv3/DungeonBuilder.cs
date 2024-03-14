using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BP.ComponentSystem;
using BP.GameConsole;
using Godot;
using Godot.Collections;

public partial class DungeonBuilder : ComponentObject
{
    [Export(PropertyHint.Dir)] private string RoomsFolderPath { get; set; }
    [Export] private ulong Seed { get; set; }  
    [Export] private ushort NumberOfRooms { get; set; }  
    [Export] private Vector3 StartPosition { get; set; }  
    private RandomNumberGenerator _random = new ();
    private DungeonTile _currentRoom = null;
    private Array<Node3D> _validConnectors = new ();
    private ushort _currentNumberOfRooms;
    private Dictionary<string, Array<PackedScene>> _tileScenes = new ()
    {
        {"block", new Array<PackedScene>()},
        {"basic", new Array<PackedScene>()},
        {"main", new Array<PackedScene>()},
        {"finish", new Array<PackedScene>()}
    };
    
    public async void Build()
    {
        Stopwatch workTime = Stopwatch.StartNew();
        GameConsole.Instance.DebugWarning($"Start generation!");
        LoadRooms(RoomsFolderPath);
        
        await Generate();
        
        workTime.Stop();
        GameConsole.Instance.DebugWarning($"Elapsed time: {workTime.Elapsed.TotalMilliseconds} ms, number of generated rooms: {_currentNumberOfRooms}");
    }
    private async Task Generate()
    {
        if (Seed != 0) _random.Seed = Seed;
        
        if (_tileScenes.Count < 1)
        {
            GameConsole.Instance.DebugError($"_roomScenes.Count == 0");
            return;
        }

        while (_currentNumberOfRooms < NumberOfRooms)
        {
            var randomCategory = _tileScenes.ElementAt(_random.RandiRange(1, _tileScenes.Count - 1)).Value;
            _currentRoom = CreateOnStage<DungeonTile>(randomCategory[_random.RandiRange(0, randomCategory.Count - 1)], StartPosition);
            
            if (_validConnectors.Count < 1)
            {
                _validConnectors.AddRange(_currentRoom.Connectors);
                continue;
            }

            ushort idCurrent = (ushort)_random.RandiRange(0, _currentRoom.Connectors.Count - 1);
            ushort idTarget = (ushort)_random.RandiRange(0, _validConnectors.Count - 1);
            
            SnapTileWithRandom(_currentRoom, _currentRoom.Connectors[idCurrent], _validConnectors[idTarget]);
            
            await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
            
            if (_currentRoom.Bounds.GetOverlappingAreas().Count > 0)
            {
                _currentRoom.QueueFree();
                GameConsole.Instance.DebugLog($"deleted: {_currentRoom.Name}");
                continue;
            }
            
            _currentRoom.Connectors.RemoveAt(idCurrent);
            _validConnectors.RemoveAt(idTarget);
            _validConnectors.AddRange(_currentRoom.Connectors);
            _currentNumberOfRooms++;
        }

        foreach (Node3D connector in _validConnectors)
        {
            _currentRoom = CreateOnStage<DungeonTile>(_tileScenes["block"][0], StartPosition);
            SnapTileWithRandom(_currentRoom, _currentRoom.Connectors[0], connector);
        }
    }
    private void LoadRooms(string path)
    {
        using DirAccess roomsFolder = DirAccess.Open(path);

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
                    string name = filenameSplit[1];
                    string category = filenameSplit[2];
                    if (_tileScenes.ContainsKey(category))
                    {
                        if (roomPath.Contains(".tscn.remap")) roomPath = roomPath.Replace(".remap", "");
                        _tileScenes[category].Add(ResourceLoader.Load<PackedScene>(roomPath));
                        
                        GameConsole.Instance.DebugLog($"LevelGenerator :: Loaded room at {GameConsole.SetColor(roomPath, "#7db39e")}, Filename: {GameConsole.SetColor(filename, "#7db39e")}, name: {GameConsole.SetColor(name, "#7db39e")}, category: {GameConsole.SetColor(category, "#7db39e")}");
                    }
                    filename = roomsFolder.GetNext();
                }
            }
            roomsFolder.ListDirEnd();
        }
    }
    private void SnapTileWithRandom(DungeonTile tile, Node3D currentConnector, Node3D targetConnector)
    {
        if(_validConnectors.Count < 1 || tile.Connectors.Count < 1) return;
        
        tile.GlobalRotation = Vector3.Zero;
        tile.Position = Vector3.Zero;
        
        tile.Position = targetConnector.GlobalPosition - (currentConnector.GlobalPosition - tile.Position);
        Vector2 vectorA = new Vector2(currentConnector.GlobalBasis.Z.X, currentConnector.GlobalBasis.Z.Z);
        Vector2 vectorB = new Vector2(targetConnector.GlobalBasis.Z.X, targetConnector.GlobalBasis.Z.Z);
        float rawAngle = (vectorA).AngleTo(vectorB);
        float angle = Mathf.Pi - rawAngle;
                    
        tile.Position = targetConnector.GlobalPosition + (tile.Position - targetConnector.GlobalPosition).Rotated(Vector3.Up, angle);
        tile.GlobalRotation = Vector3.Up * angle;
    }
    public T CreateOnStage<T>(PackedScene scene) where T : Node
    {
        T obj = scene.Instantiate<T>();
        AddChild(obj);
        return obj;
    }
    public T CreateOnStage<T>(PackedScene scene, Vector3 position) where T : Node3D
    {
        T obj = scene.Instantiate<T>();
        obj.Position = position;
        AddChild(obj);
        return obj;
    }
}