using Godot;
using System;

[GlobalClass]
public partial class PlayerController : Node
{
    public CharacterBody3D Player = null;

    [ExportGroup("Motion")]
    [Export]
    public float Gravity = 0.2f;

    [Export]
    public float Speed = 4.0f;

    [Export]
    public float JumpForce = 5.0f;

    [ExportGroup("Controls")]
    [Export]
    public float MouseSensitivity = 0.01f;

    [ExportGroup("References")]
    [Export]
    public Node3D Neck { get; private set; } = null;

    [Export]
    public Camera3D Head { get; private set; } = null;

    public override void _Ready()
    {
        Player = GetParent<CharacterBody3D>();
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");

        Player.Velocity = Neck.Basis * new Vector3(direction.X * Speed, (Input.IsActionJustPressed("jump") && Player.IsOnFloor()) ? JumpForce : Player.Velocity.Y - Gravity, direction.Y * Speed);

        Player.MoveAndSlide();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
        if (mouseMotion != null)
        {
            Neck.RotateY(-mouseMotion.Relative.X * MouseSensitivity);
            Head.Rotation = new Vector3(Mathf.Clamp(Head.Rotation.X - mouseMotion.Relative.Y * MouseSensitivity, -Mathf.Pi * 0.5f, Mathf.Pi * 0.5f), Head.Rotation.Y, Head.Rotation.Z);
        }
    }
}
