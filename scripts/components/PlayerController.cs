using BP.GameConsole;
using Godot;
using System;

[GlobalClass]
public partial class PlayerController : Node
{
    public CharacterBody3D Player = null;

    [ExportGroup("Motion")]
    [Export] public float FallAcceleration = 0.2f;
    [Export] public float WalkingSpeed = 2.5f;
    [Export] public float RunningSpeed = 5.5f;
    [Export] public float FallingSpeed = 1.5f;
    [Export] public float JumpForce = 5.0f;
    [Export(PropertyHint.Range, "0,1,0.01")] public float RoughnessOfWalk = 0.2f;
    [Export(PropertyHint.Range, "0,1,0.01")] public float RoughnessOfRun = 0.1f;
    private float _currentSpeedOfMovement;
    private float _currentSpeed;
    private float _currentRoughnessOfMovement;
    public Vector3 Direction = Vector3.Zero;
    public Vector2 InputDirectionInterpolate = Vector2.Zero;
    private Vector3 _jumpDirection = Vector3.Zero;
    private Vector3 _velocity = Vector3.Zero;
    public Vector2 InputDirection { get; private set; } = Vector2.Zero;
    public float Magnitude { get; private set; } = 0f;
    public bool IsOnFloor { get; private set; } = false;

    [ExportGroup("Controls")]
    [Export(PropertyHint.Range, "0.01,10,0.01")] public float MouseSensitivityX = 0.01f;
    [Export(PropertyHint.Range, "0.01,10,0.01")] public float MouseSensitivityY = 0.01f;
    [Export(PropertyHint.Range, "0,1,0.01")] public float RoughnessOfSensitivity = 0.25f;
    [Export(PropertyHint.Range, "-180,180,1")] public short LockAxisMinX = -40;
    [Export(PropertyHint.Range, "-180,180,1")] public short LockAxisMaxX = 90;
    private Vector2 _mouseDelta;
    private Vector3 _rotate = Vector3.Zero;
    private Vector3 _cameraRotation = Vector3.Zero;

    [ExportSubgroup("Shaking")]
    [Export(PropertyHint.Range, "0,1,0.01")] public float ShakingForcePosition = 0.01f;
    [Export(PropertyHint.Range, "0,1,0.01")] public float ShakingForceRotation = 0.95f;

    [ExportGroup("References")]
    [Export] private BoneAttachment3D _neckBone;
    [Export] public Node3D Body { get; private set; } = null;
    [Export] public Node3D CameraHandler { get; private set; } = null;
    [Export] public Node3D CameraShaker { get; private set; } = null;
    [Export] public Camera3D Camera { get; private set; } = null;

    public override void _Ready()
    {
        Player = GetParent<CharacterBody3D>();
    }
    public override void _PhysicsProcess(double delta)
    {
        CameraControll();
        BodyControll();
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        using(InputEventMouseMotion inputEventMouseMotion = @event as InputEventMouseMotion)
        {
            if (inputEventMouseMotion != null)
            {
                _mouseDelta = inputEventMouseMotion.Relative;
            }
        }
    }
    private void BodyControll()
    {
        InputDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");

        InputDirectionInterpolate = InputDirectionInterpolate.Lerp(InputDirection, RoughnessOfWalk);
        IsOnFloor = Player.IsOnFloor();
        Magnitude = Player.Velocity.Length();

        Direction = Body.Basis * new Vector3(InputDirection.X, 0, InputDirection.Y);
        _velocity.X = Mathf.Lerp(_velocity.X, (Direction.X * _currentSpeedOfMovement) + _jumpDirection.X, _currentRoughnessOfMovement);
        _velocity.Z = Mathf.Lerp(_velocity.Z, (Direction.Z * _currentSpeedOfMovement) + _jumpDirection.Z, _currentRoughnessOfMovement);

        if (IsOnFloor)
        {
            _velocity.Y = 0;
            _jumpDirection = Vector3.Zero;

            if (Input.IsActionJustPressed("jump"))
            {
                _jumpDirection = Direction * _currentSpeedOfMovement;
                _velocity.Y = JumpForce;
            }
            if (InputDirection.Length() > 0.1f)
            {
                if (Input.IsActionPressed("run"))
                {
                    _currentSpeed = RunningSpeed;
                    _currentRoughnessOfMovement = RoughnessOfRun;
                }
                else
                {
                    _currentSpeed = WalkingSpeed;
                    _currentRoughnessOfMovement = RoughnessOfWalk;
                }
            }
            else
            {
                _currentSpeed = 0;
            }
        }
        else
        {
            _velocity.Y -= FallAcceleration;
            _currentSpeed = FallingSpeed;
        }

        _currentSpeedOfMovement = _currentSpeed;
        Body.Rotation = Vector3.Up * _cameraRotation.Y;

        Player.Velocity = _velocity;
        Player.MoveAndSlide();
    }
    private void CameraControll()
    {
        _rotate.X -= (_mouseDelta.Y * MouseSensitivityX * 0.01f);
        _rotate.Y -= (_mouseDelta.X * MouseSensitivityY * 0.01f);
        _rotate.X = Mathf.Clamp(_rotate.X, Mathf.DegToRad(LockAxisMinX), Mathf.DegToRad(LockAxisMaxX));

        _cameraRotation.X = Mathf.LerpAngle(_cameraRotation.X, _rotate.X, RoughnessOfSensitivity);
        _cameraRotation.Y = Mathf.LerpAngle(_cameraRotation.Y, _rotate.Y, RoughnessOfSensitivity);

        CameraShaker.Position = _neckBone.Position.Lerp(CameraShaker.Position, ShakingForcePosition);
        CameraShaker.Rotation = _neckBone.Rotation.Lerp(CameraShaker.Rotation, ShakingForceRotation);
        CameraHandler.Rotation = Vector3.Forward * _cameraRotation.X;

        _mouseDelta = Vector2.Zero;
    }
}
