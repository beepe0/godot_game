using System.Linq;
using BP.GameConsole;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class DungeonBuilderPreset : Resource
{
    [ExportGroup("Dungeon Property Settings")]
    [Export] public ulong Seed { get; set; }
    [Export] public ushort NumberOfRooms { get; set; }
    [Export] public Vector3 StartPosition { get; set; }

    [ExportGroup("Tiles Path Settings")] 
    [Export(PropertyHint.Dir)] public Dictionary<string, string> CategoryScenes = new();
}
