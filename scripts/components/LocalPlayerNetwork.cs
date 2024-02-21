using BP.GameConsole;
using Godot;

public partial class LocalPlayerNetwork : PlayerNetwork
{
    [ExportGroup("Multiplayer")]
    [Export] private float _synchronizePlayerTickRate = 30;

    private Timer _synchronizePlayerTimer = new Timer();

    public override void Create(long id)
    {
        base.Create(id);

        AddChild(_synchronizePlayerTimer);
        _synchronizePlayerTimer.Timeout += () => Rpc(MethodName.SynchronizePlayer, _playerController.Player.Position, _playerController.Body.Rotation, _playerController.InputDirectionInterpolate, _playerController.IsOnFloor, _playerController.IsJumped, _playerController.Magnitude);
        _synchronizePlayerTimer.Start(1 / _synchronizePlayerTickRate);
    }
}