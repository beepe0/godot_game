using System.Collections.Generic;
using BP.ComponentSystem;
using BP.DebugGizmos;
using BP.GameConsole;
using Godot;

[GlobalClass]
public partial class DungeonCulling : ComponentObject
{
    [Export] public DungeonBuilder DungeonBuilder;

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
        Gizmos.Box(Target.GlobalPosition, Quaternion.Identity, Bounds, 0, Colors.Red);
        
        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            if (Input.IsPhysicalKeyPressed(Key.Space))
            {
                _position += new Vector3(0, -_velocity.Y , 0);
            }
            else
            {
                _position += new Vector3(_velocity.X, 0 , -_velocity.Y);
            }
            Target.Position = _position;
        }

        _aabb = new Aabb(Target.GlobalPosition - Bounds / 2, Bounds);
        
        _velocity = Vector2.Zero;

        // foreach (var tiles in DungeonBuilder.DungeonTiers)
        // {
        //     foreach (var tile in tiles.Tiles)
        //     {
        //         foreach (var aabb in tile.AabbBounds)
        //         {
        //             tile.Visible = _aabb.Intersects(aabb.Aabb);
        //         }
        //     }
        // }
    }
    public override void _Input(InputEvent @event)
    {
        using (InputEventMouseMotion inputEventMouseMotion = @event as InputEventMouseMotion)
        {
            if (inputEventMouseMotion != null)
            {
                _velocity = inputEventMouseMotion.Relative / 100;
            }
        }
    }
}