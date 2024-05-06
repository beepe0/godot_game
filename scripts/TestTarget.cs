using BP.GameConsole;
using Godot;

public partial class TestTarget : MeshInstance3D
{
    [Export] public CollisionShape3D CollisionShape3D;
    private float _time = 0;
    public override void _Ready()
    {
        GameConsole.Instance.DebugLog("Created!");
    }
    // public override void _PhysicsProcess(double delta)
    // {
    //     _time += (float)delta;
    //     GlobalPosition = Vector3.Right * Mathf.Cos(_time) * 10;
    // }
}