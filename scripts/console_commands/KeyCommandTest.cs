using BP.GameConsole;
using BP.GameConsole.Attribute;
using BP.GameConsole.Behaviour;
using Godot;
using System;

[Command("test")]
public partial class KeyCommandTest : Command
{
    public override void Execute(string[] keys)
    {
        GameConsole.Instance.DebugWarning($"{CommandName} : {keys[1]}");
    }
}
