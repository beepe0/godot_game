using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Walker
{
    public Stack<Vector2I> MoveHistory = new Stack<Vector2I>();

    public Vector2I Position = Vector2I.Zero;
}