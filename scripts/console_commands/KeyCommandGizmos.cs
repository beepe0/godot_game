using BP.GameConsole.Attribute;
using BP.GameConsole.Behaviour;
using BP.GameConsole;
using BP.ComponentSystem;
using Godot;
using System.Globalization;
using BP.DebugGizmos;

[Command("gizmos")]
public partial class KeyCommandGizmos : Command
{
    public override void Execute(string[] keys)
    {
        if (keys[1].Equals("set-depth-test"))
        {
            DrawGizmos.SetDepthTest(bool.Parse(keys[2]));
        }
    }
}
