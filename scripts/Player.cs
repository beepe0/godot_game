using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [ExportGroup("Animation")]
    private float _moveAnimationSensitivity = 4.0f;

    private float _moveAnimationBlend = 0.0f;

    [ExportGroup("Motion")]
    [Export]
    public float Gravity = 0.2f;

    [Export]
    public float Speed = 4.0f;

    [Export]
    public float JumpForce = 5.0f;

    [ExportGroup("Controls")]
    [Export]
    public float MouseSensitivity = 0.0001f;

    [ExportGroup("References")]
    [Export]
    public Node3D Neck { get; private set; } = null;

    [Export]
    public Camera3D Head { get; private set; } = null;

    [Export]
    public AnimationTree AnimationTree { get; private set; } = null;

    [Export]
    public AnimationPlayer AnimationPlayer { get; private set; } = null;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");

        Velocity = Neck.Basis * new Vector3(direction.X * Speed, (Input.IsActionJustPressed("jump") && IsOnFloor()) ? JumpForce : Velocity.Y - Gravity, direction.Y * Speed);

        _moveAnimationBlend = Mathf.Min((float)Mathf.MoveToward(_moveAnimationBlend, direction.Length() * Convert.ToSingle(IsOnFloor()), delta * _moveAnimationSensitivity), 1.0f);

        AnimationPlayer.SpeedScale = _moveAnimationBlend * 3.0f;
        AnimationTree.Set("parameters/blend_position", _moveAnimationBlend);

        MoveAndSlide();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
        if (mouseMotion != null)
        {
            Neck.RotateY(-mouseMotion.Relative.X * MouseSensitivity);
            Head.Rotation = new Vector3(Mathf.Clamp(Head.Rotation.X - mouseMotion.Relative.Y * MouseSensitivity, -Mathf.Pi*0.5f, Mathf.Pi*0.5f), Head.Rotation.Y, Head.Rotation.Z);
        }
    }
}
