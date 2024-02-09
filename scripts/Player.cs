using Godot;
using System;

public partial class Player : RigidBodyEx3D
{
    [ExportGroup("Animation")]
    private float _moveAnimationSensitivity = 0.25f;

    [ExportGroup("Motion")]
    [Export]
    public float Speed = 50.0f;

    [Export]
    public float JumpForce = 4.0f;

    [ExportGroup("Controls")]
    [Export]
    public float MouseSensitivity = 0.01f;

    [ExportGroup("References")]
    [Export]
    public Node3D Neck { get; private set; } = null;

    [Export]
    public Camera3D Head { get; private set; } = null;

    [Export]
    public AnimationTree AnimationTree { get; private set; } = null;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        base._IntegrateForces(state);

        if (Input.IsActionJustPressed("jump") && IsOnFloor) state.ApplyCentralImpulse(Vector3.Up * JumpForce);

        state.ApplyCentralForce(Neck.Basis * new Vector3(Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"), 0.0f, Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward")).Normalized() * Speed);

        //Animation

        float blend = Mathf.Min(state.LinearVelocity.Length() * _moveAnimationSensitivity, 1.0f);

        GD.Print(blend);

        AnimationTree.Set("parameters/blend_position", blend);
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
