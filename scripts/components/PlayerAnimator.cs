using BP.GameConsole;
using Godot;
using System;

public partial class PlayerAnimator : Node
{
    [Export] private PlayerController _controller;
    [Export] private AnimationPlayer _animationPlayer;
    [Export] private AnimationTree _animationTree;

    public override void _PhysicsProcess(double delta)
    {
     /*   GameConsole.Instance.ClearConsole();
        GameConsole.Instance.Debug($"InputDirectionInterpolate: {_controller.InputDirectionInterpolate}");
        GameConsole.Instance.Debug($"Magnitude: {_controller.Magnitude}");
        GameConsole.Instance.Debug($"IsOnFloor: {_controller.IsOnFloor}");*/

        _animationTree.Set("parameters/conditions/isWalk", _controller.IsOnFloor && _controller.Magnitude > 0.1f && _controller.Magnitude < 2.9f);
        _animationTree.Set("parameters/_walk_blendSpace/blend_position", _controller.InputDirectionInterpolate);
        _animationTree.Set("parameters/conditions/isRun", _controller.IsOnFloor && _controller.Magnitude > 2.9f);
        _animationTree.Set("parameters/conditions/isJump", _controller.IsOnFloor && _controller.IsJumped);
        _animationTree.Set("parameters/conditions/isFall", !_controller.IsOnFloor);
        _animationTree.Set("parameters/conditions/isIdle", _controller.IsOnFloor && _controller.Magnitude < 1);
    }
}
