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
        GameConsole.Instance.Debug($"{CommandName} : {keys[1]}");
        GameConsole.Instance.DebugLog($"{CommandName} : {keys[1]}");
        GameConsole.Instance.DebugWarning($"{CommandName} : {keys[1]}");
        GameConsole.Instance.DebugError($"{CommandName} : {keys[1]}");
    }
}
