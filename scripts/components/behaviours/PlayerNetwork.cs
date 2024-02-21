using BP.GameConsole;
using Godot;

public partial class PlayerNetwork : Node
{
    [ExportGroup("References")]
    [Export] protected PlayerStats _playerStats;
    [Export] protected PlayerAnimator _playerAnimator;
    [Export] protected PlayerController _playerController;

    public virtual void Create(long id)
    {
        Name = id.ToString();

        _playerStats.PlayeId = id;
        _playerStats.PlayerName = $"PLAYER_{id}";
        _playerController.Player.Name = _playerStats.PlayerName;
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferChannel = 0, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
    public void SynchronizePlayer(Vector3 position, Vector3 rotation, Vector2 inputDirectionInterpolate, bool isOnFloor, bool isJumped, float magnitude)
    {
        _playerController.FromRemotePlayer(position, rotation, inputDirectionInterpolate, isOnFloor, isJumped, magnitude);
    }

}