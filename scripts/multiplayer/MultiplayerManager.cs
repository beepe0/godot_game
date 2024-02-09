using Godot;
using System;

public partial class MultiplayerManager : Node
{
    [ExportGroup("Environment")]
    [Export] private PackedScene _localPlayerScene;
    [Export] private PackedScene _remotePlayerScene;

    public override void _EnterTree()
    {
        Game.MultiplayerManager = this;
    }
    public override void _Ready()
    {
        Multiplayer.PeerConnected += (id) =>
        {
            InitPlayer<LocalPlayer>(_remotePlayerScene, id);
        };
    }
    private void InitPlayer<TPlayer>(PackedScene playerScene, long id) where TPlayer : Player
    {
        TPlayer player = playerScene.Instantiate<TPlayer>();
        //player.Start(id);

        AddChild(player);
    }
    public TPlayer GetPlayer<TPlayer>(long id) where TPlayer : Player
    {
        return GetNode<TPlayer>(id.ToString());
    }
    public void StartHost(ushort port, ushort maxClients)
    {
        ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
        peer.CreateServer(port, maxClients);
        Multiplayer.MultiplayerPeer = peer;

        InitPlayer<LocalPlayer>(_localPlayerScene, Multiplayer.GetUniqueId());
    }
    public void StartClient(string ip, ushort port)
    {
        ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
        peer.CreateClient(ip, port);
        Multiplayer.MultiplayerPeer = peer;

        InitPlayer<LocalPlayer>(_localPlayerScene, Multiplayer.GetUniqueId());
    }
}
