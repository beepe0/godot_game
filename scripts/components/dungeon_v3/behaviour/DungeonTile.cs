using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BP.ComponentSystem;
using BP.DebugGizmos;
using BP.GameConsole;
using Godot;
using Godot.Collections;

public partial class DungeonTile : Node3D
{
    [ExportGroup("Preset")] 
    [Export] public DungeonTileCategoryPreset Preset;
    [ExportGroup("Properties")] 
    [Export] public bool IsAutoDetectConnectors = true;
    [Export] public Array<Node3D> Connectors; 
    
    public Area3D AreaBounds;
    public List<TileBounds> AabbBounds;
    protected DungeonBuilder DungeonBuilder;
    
    public override void _Ready()
    {
        AreaBounds = GetNodeOrNull<Area3D>("Bounds");

        Array<Node> aabbBounds = AreaBounds.GetChildren();
        AabbBounds = new();
        
        if (IsAutoDetectConnectors)
        {
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
                
            if (tile.AreaBounds.GetOverlappingAreas().Count > 0)
            { 
                tile.QueueFree();
                return null;
            }
            tile.OnInit();
            
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
    private void Snap(Node3D currentConnector, Node3D targetConnector)
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
    public virtual void OnDrawGizmos(ushort id)
    {
        foreach (var aabb in AabbBounds)
        {
            Gizmos.Box(aabb.Transform3D, aabb.Aabb.Size, 0, Colors.Blue);
        }
    }
    public virtual void OnInit()
    {
        foreach (CollisionShape3D bound in AreaBounds.GetChildren())
        {
            BoxShape3D boxShape3D = bound.Shape as BoxShape3D;
            //AabbBounds.Add(new TileBounds(bound.GlobalTransform, new Aabb(bound.GlobalPosition - boxShape3D.Size / 2, bound.GlobalTransform.)));
        }
    }
    public struct TileBounds
    {
        public Transform3D Transform3D;
        public Aabb Aabb;

        public TileBounds(Transform3D transform3D, Aabb aabb)
        {
            Transform3D = transform3D;
            Aabb = aabb;
        }
    }
}
