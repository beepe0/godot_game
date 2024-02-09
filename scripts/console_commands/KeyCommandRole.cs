using BP.GameConsole.Attribute;
using BP.GameConsole.Behaviour;
using BP.GameConsole;
using Godot;

[Command("role")]
public partial class KeyCommandRole : Command
{
    public override void Execute(string[] keys)
    {
        if (keys[1].Equals("c")) Game.MultiplayerManager.StartClient(keys[2], ushort.Parse(keys[3]));
        else if (keys[1].Equals("h")) Game.MultiplayerManager.StartHost(ushort.Parse(keys[2]), ushort.Parse(keys[3]));
    }
}
