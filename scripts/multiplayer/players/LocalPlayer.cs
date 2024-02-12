using Godot;
using System;

public partial class LocalPlayer : Player
{
    [ExportGroup("Animation")]
    private float _moveAnimationSensitivity = 4.0f;

    private float _moveAnimationBlend = 0.0f;


}