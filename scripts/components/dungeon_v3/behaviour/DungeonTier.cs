using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BP.DebugGizmos;
using Godot;

public partial class DungeonTier : Node
{
    public DungeonBuilder DungeonBuilder;
    
    public readonly List<DungeonTile> ValidTiles = new();
    public readonly List<DungeonTile> Tiles = new();

    public async Task AddTiles(DungeonTile targetRoom)
    {
        if (targetRoom == null) return;
        
        List<DungeonTile> neighbours = new();
        foreach (Node3D targetConnector in targetRoom.Connectors)
        {
            if(DungeonBuilder.CurrentNumberOfTiers >= DungeonBuilder.DungeonBuilderPreset.NumberOfTiers) break;
                
            DungeonTile currentRoom = await targetRoom.InitTileOrNull(targetConnector);
                
            if(currentRoom != null) neighbours.Add(currentRoom);
        }
        ValidTiles.Remove(targetRoom);

        if (neighbours.Count > 0)
        {
            Tiles.AddRange(neighbours);
            ValidTiles.AddRange(neighbours);
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        for (ushort i = 0; i < Tiles.Count; i++)
        {
            Tiles[i].DrawGizmos(i);
        }
    }
}