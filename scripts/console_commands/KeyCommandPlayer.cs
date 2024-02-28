using BP.GameConsole.Attribute;
using BP.GameConsole.Behaviour;
using BP.GameConsole;
using GH.ComponentSystem;
using Godot;
using System.Globalization;

[Command("player")]
public partial class KeyCommandPlayer : Command
{
    private PlayerController playerController;

    public override void Initialize(string name)
    {
        base.Initialize(name);
    }
    public override void Execute(string[] keys)
    {
        if (playerController == null) 
        {
            if (keys[1].Equals("create"))
            {
                MultiplayerManager.Instance.InitPlayer(MultiplayerManager.Instance._localPlayerScene, 0);
            }

            playerController = GetTree().GetFirstNodeInGroup("LocalPlayer").GetComponent<PlayerController>();
        }
        else if (keys[1].Equals("noclip"))
        {
            playerController.State = playerController.State == PlayerController.StatePlayerController.Local ? PlayerController.StatePlayerController.Noclip : PlayerController.StatePlayerController.Local;
        }
        else if (keys[1].Equals("noclip-speed"))
        {
            playerController.NoclipingSpeed = float.Parse(keys[2], CultureInfo.InvariantCulture.NumberFormat);
        }
        else if (keys[1].Equals("sensitivity"))
        {
            playerController.MouseSensitivityX = float.Parse(keys[2], CultureInfo.InvariantCulture.NumberFormat);
            playerController.MouseSensitivityY = float.Parse(keys[2], CultureInfo.InvariantCulture.NumberFormat);
        }
    }
}
