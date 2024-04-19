using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BP.ComponentSystem;
using BP.DebugGizmos;
using Godot;
using Godot.Collections;

public partial class DungeonTile : Node3D
{
    [ExportGroup("Preset")] 
    [Export] public DungeonTileCategoryPreset Preset;
    [ExportGroup("Properties")] 
    [Export] public bool IsAutoDetect = true;
    [Export] public Array<Node3D> Connectors;
    [Export] public Area3D Bounds;

    public DungeonBuilder DungeonBuilder;
    
    public override void _Ready()
    {
        if (IsAutoDetect)
        {
            Bounds = GetNodeOrNull<Area3D>("Bounds");

            Array<Node> connectors = GetNodeOrNull<Node3D>("Connectors").GetChildren();
            if (connectors != null && connectors.Count > 0)
            {
                Connectors = new();

                foreach (var node in connectors)
                {
                    Connectors.Add(node as Node3D);
                }
            }
        }

        DungeonBuilder = ComponentSystem.GetComponentSystemWithTag(Preset.GenerationName).GetComponent<DungeonBuilder>();

    }
    public async Task<DungeonTile> InitTileOrNull(Node3D targetSnap = null)
    {
        DungeonTile tile = InitTile();
        
        if (tile == null) return null;
        
        if (targetSnap != null)
        {
            Node3D currentConnector = tile.Connectors[(ushort)DungeonBuilder.Random.RandiRange(0, tile.Connectors.Count - 1)];
            tile.Snap(currentConnector, targetSnap);
                
            await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
                
            if (tile.Bounds.GetOverlappingAreas().Count > 0)
            {
                tile.QueueFree();
                return null;
            }
        
            currentConnector.Visible = false;
            targetSnap.Visible = false;
        }
        
        tile.Preset.CurrentNumberOfTilesPerTier++;
        DungeonBuilder.CurrentNumberOfTiles++;
        
        return tile;
    }
    private DungeonTile InitTile()
    {
        DungeonTileCategoryPreset preset = null;
        do
        {
            if (Preset.AvailableTileScenes.Count < 1) return null;

            preset = GetMin(Preset.AvailableTileScenes);

            if (preset.CurrentNumberOfTilesPerTier >= preset.TargetNumberOfTilesPerTier)
            {
                Preset.AvailableTileScenes.Remove(preset);
                Preset.UnavailableTileScenes.Add(preset);
                preset = null;
            }
            
        } while (preset == null);
        
        return preset.PackedScenes[(ushort)DungeonBuilder.Random.RandiRange(0, preset.PackedScenes.Count - 1)].CreateOnStage<DungeonTile>(DungeonBuilder.DungeonTiers[DungeonBuilder.CurrentNumberOfTiers], DungeonBuilder.DungeonBuilderPreset.StartPosition);
    }
    private DungeonTileCategoryPreset GetMin(List<DungeonTileCategoryPreset> list)
    {
        DungeonTileCategoryPreset minTile = null;
        ushort minPriority = ushort.MaxValue;
        
        foreach (var t in list)
        {
            if (t.Priority < minPriority)
            {
                minTile = t;
                minPriority = t.Priority;
            }
        }

        return minTile;
    }

    public virtual void DrawGizmos(ushort id)
    {
        foreach (var node in Bounds.GetChildren())
        {
            CollisionShape3D coll = ((CollisionShape3D)node);
            Gizmos.Box(coll.GlobalPosition, Quaternion.Identity, ((BoxShape3D)coll.Shape).Size, 0, Colors.Gray);
            Gizmos.SolidBox(coll.GlobalPosition, Quaternion.Identity, Vector3.One / 3, 0, Colors.Gray);
        }
    }
    public void Snap(Node3D currentConnector, Node3D targetConnector)
    {
        if(Connectors.Count < 1) return;
        
        Rotation = Vector3.Zero;
        
        GlobalPosition = targetConnector.GlobalPosition - (currentConnector.GlobalPosition - GlobalPosition);
        Vector2 vectorA = new Vector2(currentConnector.GlobalBasis.Z.X, currentConnector.GlobalBasis.Z.Z);
        Vector2 vectorB = new Vector2(targetConnector.GlobalBasis.Z.X, targetConnector.GlobalBasis.Z.Z);
        float rawAngle = (vectorA).AngleTo(vectorB);
        float angle = Mathf.Pi - rawAngle;
                    
        GlobalPosition = targetConnector.GlobalPosition + (GlobalPosition - targetConnector.GlobalPosition).Rotated(Vector3.Up, angle);
        Rotation = Vector3.Up * angle;
    }
}
