using System.Collections.Generic;
using BP.ComponentSystem;
using BP.DebugGizmos;
using BP.GameConsole;
using Godot;

[GlobalClass]
public partial class DungeonCulling : ComponentObject
{
    public readonly List<DungeonTile> DungeonTiles = new List<DungeonTile>();

    [Export] public Node3D Target;
    [Export] public Vector3 Bounds;
    
    private Aabb _aabb;
    private Vector2 _velocity;
    private Vector3 _position;
    public void Begin()
    {
        Target = GetTree().GetFirstNodeInGroup("TargetTest") as Node3D;
    }
    public override void _PhysicsProcess(double delta)
    {
        if(Target == null) return;
        
        Gizmos.SolidSphere(Target.GlobalPosition, Quaternion.Identity, Target.Scale / 2, 0, Colors.Red);
        Gizmos.Box(Target.GlobalPosition, Quaternion.Identity, Bounds / 2, 0, Colors.Burlywood);
        
        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            if (Input.IsPhysicalKeyPressed(Key.Space))
            {
                _position += new Vector3(0, -_velocity.Y , 0);
            }
            else
            {
                _position += new Vector3(-_velocity.Y, 0 , _velocity.X);
            }
            Target.Position = _position;
        }

        _aabb = new Aabb(Target.GlobalPosition - Bounds / 2, Bounds);

        _velocity = Vector2.Zero;
        
        foreach (var tile in DungeonTiles)
        {
            Gizmos.SolidSphere(tile.GlobalPosition, Quaternion.Identity, Vector3.One / 2, 0, Colors.Cyan);
            //Gizmos.Text3D(tile.Category, tile.GlobalPosition, 0, Colors.Orange);
        
            tile.Visible = _aabb.HasPoint(tile.GlobalPosition);
        }
    }
    public override void _Input(InputEvent @event)
    {
        using (InputEventMouseMotion inputEventMouseMotion = @event as InputEventMouseMotion)
        {
            if (inputEventMouseMotion != null)
            {
                _velocity = inputEventMouseMotion.Velocity / 1000;
            }
        }
    }
}