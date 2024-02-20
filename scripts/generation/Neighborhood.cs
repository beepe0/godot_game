using Godot;
using System;
using System.Linq;

public static class Neighborhood
{
    public static readonly Vector2I[] Manhattan = new Vector2I[] { Vector2I.Right, Vector2I.Left, Vector2I.Down, Vector2I.Up };
    public static readonly Vector2I[] Moore = Manhattan.Concat(new Vector2I[] { new Vector2I(1, 1), new Vector2I(-1, 1), new Vector2I(1, -1), new Vector2I(-1, -1) }).ToArray();
}