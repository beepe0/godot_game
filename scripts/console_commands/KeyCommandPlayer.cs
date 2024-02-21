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
        playerController = GetTree().GetFirstNodeInGroup("LocalPlayer").GetComponent<PlayerController>();

        if (playerController == null) { GameConsole.Instance.DebugError($"The player does not exist!"); return; }
        if (keys[1].Equals("noclip"))
        {
            playerController.State = playerController.State == PlayerController.StatePlayerController.Normal ? PlayerController.StatePlayerController.Noclip : PlayerController.StatePlayerController.Normal;
        }
        else if (keys[1].Equals("speed"))
        {
            playerController.NoclipingSpeed = float.Parse(keys[2]);
        }
        else if (keys[1].Equals("sensitivity"))
        {
            playerController.MouseSensitivityX = float.Parse(keys[2], CultureInfo.InvariantCulture.NumberFormat);
            playerController.MouseSensitivityY = float.Parse(keys[2], CultureInfo.InvariantCulture.NumberFormat);
        }
    }
}
