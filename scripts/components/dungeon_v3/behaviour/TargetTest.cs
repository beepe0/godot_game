using BP.DebugGizmos;
using BP.GameConsole;
using Godot;

[GlobalClass]
public partial class TargetTest : Marker3D
{
    [Export] public float Range;
    private Vector2 _velocity = Vector2.Zero;
    private Vector3 _position;
    public override void _PhysicsProcess(double delta)
    {
        DrawGizmos.SolidSphere(GlobalPosition, Quaternion.Identity, Scale/2, 0, Colors.Red);
        DrawGizmos.Sphere(GlobalPosition, Quaternion.Identity, Vector3.One * Range, 0, Colors.Burlywood);

        DrawGizmos.Text2D($"Mouse Delta: {_velocity}", new Vector2(100, 220));

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
            Position = _position;
        }
        _velocity = Vector2.Zero;
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