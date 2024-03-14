using System.Collections.Generic;
using Godot;

public class DungeonWalker
{
    public Stack<Vector2I> MoveHistory = new Stack<Vector2I>();
    public Vector2I Position = Vector2I.Zero;
}