using BP.GameConsole.Attribute;
using BP.GameConsole.Behaviour;
using Godot;

[Command("network")]
public partial class KeyCommandNetwork : Command
{
    public override void Execute(string[] keys)
    {
        if (keys[1].Equals("client"))
        {
            MultiplayerManager.Instance.StartClient(keys);
        }
        else if (keys[1].Equals("server"))
        {
            MultiplayerManager.Instance.StartServer(keys);
        }
    }
}
