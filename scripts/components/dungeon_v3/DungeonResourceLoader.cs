using System;
using System.Collections.Generic;
using BP.ComponentSystem;
using BP.GameConsole;
using Godot;

public partial class DungeonResourceLoader : ComponentObject
{
    [Export] public DungeonPreset DungeonPreset;
    public readonly Dictionary<DungeonTile.DungeonTileCategory, DungeonTileProperties> TileScenes = new ();

    public ushort LoadTiles(Dictionary<DungeonTile.DungeonTileCategory, DungeonTileProperties> tileScenes)
    {
        try
        {
            foreach (DungeonCategoryPreset preset in DungeonPreset.TileScenes)
            {
                using DirAccess roomsFolder = DirAccess.Open(preset.RoomsFolderPath);

                if (roomsFolder != null)
                {
                    roomsFolder.ListDirBegin();

                    string filename = roomsFolder.GetNext();

                    while (filename != "")
                    {
                        if (!roomsFolder.CurrentIsDir())
                        {
                            string roomPath = roomsFolder.GetCurrentDir().PathJoin(filename);
                
                            roomPath = roomPath.Contains(".tscn.remap") ? roomPath.Replace(".remap", "") : roomPath;
                            
                            if (tileScenes.ContainsKey(preset.TilesCategory))
                            {
                                tileScenes[preset.TilesCategory].PackedScenes.Add(ResourceLoader.Load<PackedScene>(roomPath));
                            }
                            else
                            {
                                tileScenes.Add(preset.TilesCategory, new DungeonTileProperties()
                                {
                                    PackedScenes = new List<PackedScene>(){ResourceLoader.Load<PackedScene>(roomPath)},
                                    NumberOfTilesPerTier = preset.TargetNumberOfTilesPerTier,
                                    Priority = preset.Priority
                                });
                            }

                            GameConsole.Instance.DebugLog($"LevelGenerator :: Loaded room at {GameConsole.SetColor(roomPath, "#7db39e")}, Filename: {GameConsole.SetColor(filename, "#7db39e")}, Category: {GameConsole.SetColor(preset.TilesCategory.ToString(), "#7db39e")}");
                            filename = roomsFolder.GetNext();
                        }
                    }
                    roomsFolder.ListDirEnd();
                }
            }
        }
        catch (Exception e)
        {
            GameConsole.Instance.DebugError($"{GetType()} :: {e}");
        }
        
        return (ushort)tileScenes.Count;
    }
    
    public class DungeonTileProperties
    {
        public List<PackedScene> PackedScenes;
        public ushort NumberOfTilesPerTier;
        public ushort Priority;
        public ushort CurrentNumberOfTilesPerTier;
    }
}