using Godot;
using System.Collections.Generic;

public class MapWalker
{
    public Stack<Vector2I> MoveHistory = new Stack<Vector2I>();

    public Vector2I Position = Vector2I.Zero;
}