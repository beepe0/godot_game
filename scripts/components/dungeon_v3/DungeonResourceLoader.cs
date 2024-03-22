using System;
using System.Collections.Generic;
using BP.ComponentSystem;
using BP.GameConsole;
using Godot;

public partial class DungeonResourceLoader : ComponentObject
{
    [Export] public DungeonBuilderPreset DungeonBuilderPreset;

    public ushort LoadResources(Dictionary<string, DungeonTileCategory> tileScenes)
    {
        try
        {
            GameConsole.Instance.DebugLog($"{GetType()} :: {GameConsole.SetColor("Loading the categories!", "eb8334")}");

            foreach (var preset in DungeonBuilderPreset.CategoryScenes)
            {
                string path = preset.Value.Contains(".tscn.remap") ? preset.Value.Replace(".remap", "") : preset.Value;
                            
                if (!tileScenes.ContainsKey(preset.Key))
                {
                    tileScenes.Add(preset.Key, ResourceLoader.Load<PackedScene>(path).CreateOnStage<DungeonTileCategory>(this));
                    GameConsole.Instance.DebugLog($"{GetType()} :: Loaded room at {GameConsole.SetColor(path, "#7db39e")}, Category: {GameConsole.SetColor(preset.Key, "#7db39e")}");
                }
            }
            
            GameConsole.Instance.DebugLog($"{GetType()} :: {GameConsole.SetColor("Loading the tiles!", "eb8334")}");
            
            foreach (var preset in tileScenes)
            {
                using DirAccess roomsFolder = DirAccess.Open(preset.Value.DungeonTileCategoryPreset.RoomsFolderPath);
                
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
                            
                            preset.Value.PackedScenes.Add(ResourceLoader.Load<PackedScene>(roomPath));
                
                            GameConsole.Instance.DebugLog($"{GetType()} :: Loaded room at {GameConsole.SetColor(roomPath, "#7db39e")}, Filename: {GameConsole.SetColor(filename, "#7db39e")}, Category: {GameConsole.SetColor(preset.Key.ToString(), "#7db39e")}");
                            filename = roomsFolder.GetNext();
                        }
                    }
                    roomsFolder.ListDirEnd();
                }
            }
            GameConsole.Instance.DebugLog($"{GetType()} :: {GameConsole.SetColor("The loading was completed successfully!", "21799e")}");

        }
        catch (Exception e)
        {
            GameConsole.Instance.DebugLog($"{GetType()} :: {GameConsole.SetColor("The loading was completed unsuccessfully!", "963636")} {e}");
        }
        
        return (ushort)tileScenes.Count;
    }
}