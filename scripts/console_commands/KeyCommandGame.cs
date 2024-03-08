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
            GetTree().GetFirstNodeInGroup("Mirrors").ProcessMode = ProcessModeEnum.Pausable;
        }
    }
}
