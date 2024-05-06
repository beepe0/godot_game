using BP.GameConsole.Attribute;
using BP.GameConsole.Behaviour;
using BP.GameConsole;
using BP.ComponentSystem;
using Godot;
using System.Globalization;

[Command("game")]
public partial class KeyCommandGame : Command
{
    public override void Execute(string[] keys)
    {
        if (keys[1].Equals("mirror"))
        {
            ((Camera3D)GetTree().GetFirstNodeInGroup("Mirrors")).Current = !((Camera3D)GetTree().GetFirstNodeInGroup("Mirrors")).Current;
        }
        else if (keys[1].Equals("generate-dungeon"))
        {
            ComponentSystem.GetComponentSystemWithTag("CityGeneration").GetComponent<DungeonBuilder>().Build();
        }
        else if (keys[1].Equals("target-fps"))
        {
            Engine.MaxFps = int.Parse(keys[2]);
        }
    }
}
