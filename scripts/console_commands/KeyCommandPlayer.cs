using BP.GameConsole.Attribute;
using BP.GameConsole.Behaviour;
using BP.GameConsole;
using GH.ComponentSystem;
using Godot;

[Command("player")]
public partial class KeyCommandPlayer : Command
{
    private PlayerController playerController;

    public override void Initialize(string name)
    {
        base.Initialize(name);
        playerController = GetNodeOrNull<PlayerController>("/root/Game/Player/PlayerController");
    }
    public override void Execute(string[] keys)
    {
        if (playerController == null) { GameConsole.Instance.DebugError($"The player does not exist!");  return; }

        if (keys[1].Equals("noclip"))
        {
            playerController.State = playerController.State == PlayerController.StatePlayerController.Normal ? PlayerController.StatePlayerController.Noclip : PlayerController.StatePlayerController.Normal;
        }
        else if (keys[1].Equals("speed"))
        {
            playerController.NoclipingSpeed = float.Parse(keys[2]);
        }
    }
}
