using BP.GameConsole.Attribute;
using BP.GameConsole.Behaviour;
using BP.GameConsole;
using BP.ComponentSystem;
using Godot;
using System.Globalization;

[Command("player")]
public partial class KeyCommandPlayer : Command
{
    private PlayerController _playerController;
    
    public override void Execute(string[] keys)
    {
        if (_playerController == null) 
        {
            if (keys[1].Equals("create"))
            {
                MultiplayerManager.Instance.InitPlayer(MultiplayerManager.Instance._localPlayerScene, 0);
            }

            _playerController = GetTree().GetFirstNodeInGroup("LocalPlayer").GetComponent<PlayerController>();
        }
        else if (keys[1].Equals("noclip"))
        {
            _playerController.State = _playerController.State == PlayerController.StatePlayerController.Local ? PlayerController.StatePlayerController.Noclip : PlayerController.StatePlayerController.Local;
        }
        else if (keys[1].Equals("noclip-speed"))
        {
            _playerController.NoclipingSpeed = float.Parse(keys[2], CultureInfo.InvariantCulture.NumberFormat);
        }
        else if (keys[1].Equals("sensitivity"))
        {
            _playerController.MouseSensitivityX = float.Parse(keys[2], CultureInfo.InvariantCulture.NumberFormat);
            _playerController.MouseSensitivityY = float.Parse(keys[2], CultureInfo.InvariantCulture.NumberFormat);
        }
    }
}
