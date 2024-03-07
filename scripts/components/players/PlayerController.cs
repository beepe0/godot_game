using BP.GameConsole;
using BP.ComponentSystem;
using Godot;
using System;

public partial class PlayerController : Node
{
    public CharacterBody3D Player = null;

    [ExportGroup("Motion")]
    [Export] public float FallAcceleration = 0.2f;
    [Export] public float WalkingSpeed = 2.5f;
    [Export] public float RunningSpeed = 5.5f;
    [Export] public float FallingSpeed = 1.5f;
    [Export] public float NoclipingSpeed = 40f;
    [Export] public float JumpForce = 5.0f;
    [Export(PropertyHint.Range, "0,1,0.01")] public float RoughnessOfWalk = 0.2f;
    [Export(PropertyHint.Range, "0,1,0.01")] public float RoughnessOfRun = 0.1f;
    public float CurrentSpeedOfMovement;
    public float CurrentRoughnessOfMovement;
    public float CurrentFallAcceleration;
    public Vector3 Velocity = Vector3.Zero;
    public Vector3 JumpDirection = Vector3.Zero;
    public Vector3 Direction { get; private set; } = Vector3.Zero;
    public Vector2 InputDirectionInterpolate { get; private set; } = Vector2.Zero;
    public Vector2 InputDirection { get; private set; } = Vector2.Zero;
    public Vector3 Position { get; private set; } = Vector3.Zero;
    public Vector3 Rotation { get; private set; } = Vector3.Zero;
    public float Magnitude { get; private set; } = 0f;
    public bool IsOnFloor { get; private set; } = false;
    public bool IsJumped { get; private set; } = false;

    [ExportGroup("Control")]
    [Export(PropertyHint.Range, "0.01,10,0.01")] public float MouseSensitivityX = 0.01f;
    [Export(PropertyHint.Range, "0.01,10,0.01")] public float MouseSensitivityY = 0.01f;
    [Export(PropertyHint.Range, "0,1,0.01")] public float RoughnessOfSensitivity = 0.25f;
    [Export(PropertyHint.Range, "-180,180,1")] public short LockAxisMinX = -40;
    [Export(PropertyHint.Range, "-180,180,1")] public short LockAxisMaxX = 90;
    [Export(PropertyHint.Range, "-360,360,1")] public short LockArmAxisMinX = -150;
    [Export(PropertyHint.Range, "-360,360,1")] public short LockArmAxisMaxX = 150;
    private Vector2 _mouseDelta;
    private Vector3 _cameraRotationDelta = Vector3.Zero;

    [ExportSubgroup("Dependence on Armature")]
    [Export(PropertyHint.Range, "0,1,0.01")] public float PowerOfAddictionPositionCamera = 0.01f;
    [Export(PropertyHint.Range, "0,1,0.01")] public float PowerOfAddictionRotationCamera = 0.1f;
    [Export(PropertyHint.Range, "0,1,0.01")] public float PowerOfAddictionPositionArm = 0.05f;
    [Export(PropertyHint.Range, "0,1,0.01")] public float PowerOfAddictionRotationArm = 0.03f;

    [ExportGroup("State")]
    [Export] public StatePlayerController State = StatePlayerController.Noclip;

    [ExportGroup("References")]
    [Export] private RayCast3D _interactRay;
    [Export] private BoneAttachment3D _boneAttachmentNeck;
    [Export] private BoneAttachment3D _boneAttachmentBreast;
    [Export] private BoneAttachment3D _boneAttachmentShoulder;
    [Export] public Node3D ArmHandler;
    [Export] public Node3D Body { get; private set; } = null;
    [Export] public Node3D CameraHandler { get; private set; } = null;
    [Export] public Node3D CameraShaker { get; private set; } = null;
    [Export] public Camera3D Camera { get; private set; } = null;
    [Export] public CollisionShape3D Collider { get; private set; } = null;

    public override void _Ready()
    {
        Player = GetParent<CharacterBody3D>();
    }
    public override void _PhysicsProcess(double delta)
    {
        if (GameConsole.Instance.Visible) return;
        switch(State)
        {
            case StatePlayerController.Local: LocalControl(); break;
            case StatePlayerController.Remote: RemoteControl(); break;
            case StatePlayerController.Noclip: NoclipControl(); break;
        }
    }
    public override void _Input(InputEvent @event)
    {
        using(InputEventMouseMotion inputEventMouseMotion = @event as InputEventMouseMotion)
        {
            if (inputEventMouseMotion != null)
            {
                _mouseDelta = inputEventMouseMotion.Relative;
            }
        }
    }
    private void InteractWith()
    {
        if (Input.IsActionJustPressed("interact") && _interactRay.IsColliding())
        {
            if (((Node)_interactRay.GetCollider()).GetParent().GetComponent<Interactable>() is { } interactable) interactable.Interact(Player);
        }
    }
    private void LocalCameraControl()
    {
        float rX = (_mouseDelta.Y * MouseSensitivityX * 0.01f);
        float rY = (_mouseDelta.X * MouseSensitivityY * 0.01f);

        _cameraRotationDelta = _cameraRotationDelta.Lerp(new Vector3(rX, rY, 0), RoughnessOfSensitivity);

        CameraShaker.Position = CameraShaker.Position.Lerp(_boneAttachmentNeck.Position, PowerOfAddictionPositionCamera);
        CameraShaker.Rotation = CameraShaker.Rotation.Lerp(_boneAttachmentNeck.Rotation, PowerOfAddictionRotationCamera);
        
        ArmHandler.Position = ArmHandler.Position.Lerp(_boneAttachmentBreast.Position, PowerOfAddictionPositionArm);
        ArmHandler.Rotation = ArmHandler.Rotation.Lerp(_boneAttachmentBreast.Rotation, PowerOfAddictionRotationArm);

        CameraHandler.RotateX(-_cameraRotationDelta.X);
        Body.RotateY(-_cameraRotationDelta.Y);
        _boneAttachmentShoulder.RotateX(-_cameraRotationDelta.X);

        CameraHandler.Rotation = CameraHandler.Rotation.Clamp(Vector3.Right * Mathf.DegToRad(LockAxisMinX), Vector3.Right * Mathf.DegToRad(LockAxisMaxX));
        _boneAttachmentShoulder.Rotation = _boneAttachmentShoulder.Rotation.Clamp(Vector3.Right * Mathf.DegToRad(LockArmAxisMinX), Vector3.Right * Mathf.DegToRad(LockArmAxisMaxX));
    
        _mouseDelta = Vector2.Zero;
    }
    private void RemoteCameraControl()
    {
        Body.Rotation = Body.Rotation.Lerp(Rotation, 0.33f);
    }
    private void LocalControl()
    {
        LocalCameraControl();
        InteractWith();

        Collider.Disabled = false;
        InputDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");

        InputDirectionInterpolate = InputDirectionInterpolate.Lerp(InputDirection, RoughnessOfWalk);
        IsOnFloor = Player.IsOnFloor();
        Magnitude = Player.Velocity.Length();

        Direction = Body.Basis * new Vector3(InputDirection.X, 0, InputDirection.Y);
        Velocity.X = Mathf.Lerp(Velocity.X, (Direction.X * CurrentSpeedOfMovement) + JumpDirection.X, CurrentRoughnessOfMovement);
        Velocity.Z = Mathf.Lerp(Velocity.Z, (Direction.Z * CurrentSpeedOfMovement) + JumpDirection.Z, CurrentRoughnessOfMovement);

        if (IsOnFloor)
        {
            IsJumped = false;
            Velocity.Y = 0;
            JumpDirection = Vector3.Zero;

            if (Input.IsActionJustPressed("jump"))
            {
                IsJumped = true;
                JumpDirection = Direction * CurrentSpeedOfMovement;
                Velocity.Y = JumpForce;
            }
            if (InputDirection.Length() > 0.1f)
            {
                if (Input.IsActionPressed("run"))
                {
                    CurrentSpeedOfMovement = RunningSpeed;
                    CurrentRoughnessOfMovement = RoughnessOfRun;
                }
                else
                {
                    CurrentSpeedOfMovement = WalkingSpeed;
                    CurrentRoughnessOfMovement = RoughnessOfWalk;
                }
            }
            else
            {
                CurrentSpeedOfMovement = 0;
            }
        }
        else
        {
            CurrentFallAcceleration = FallAcceleration;
            Velocity.Y -= CurrentFallAcceleration;
            CurrentSpeedOfMovement = FallingSpeed;
        }
        Player.Velocity = Velocity;
        Player.MoveAndSlide();
    }
    private void RemoteControl()
    {
        RemoteCameraControl();
        Collider.Disabled = true;
        Player.Position = Player.Position.Lerp(Position, 0.3f);
    }
    private void NoclipControl()
    {
        LocalCameraControl();
        Collider.Disabled = true;
        CurrentSpeedOfMovement = NoclipingSpeed;

        InputDirection = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
        Direction = (Body.Basis * CameraHandler.Basis) * new Vector3(InputDirection.X, 0, InputDirection.Y);
        Velocity = Direction * CurrentSpeedOfMovement;
        Player.Velocity = Velocity;
        Player.MoveAndSlide();
    }
    public void FromRemotePlayer(Vector3 position, Vector3 rotation, Vector2 inputDirectionInterpolate, bool isOnFloor, bool isJumped, float magnitude)
    {
        Position = position;
        Rotation = rotation;
        InputDirectionInterpolate = inputDirectionInterpolate;

        IsOnFloor = isOnFloor;
        IsJumped = isJumped;
        Magnitude = magnitude;
    }
    public void TeleportTo(Vector3 to)
    {
        GameConsole.Instance.Debug($"Teleported to: {to}");
        Player.Position = to;
    }
    public enum StatePlayerController
    {
        Local,
        Remote,
        Noclip
    }
}

