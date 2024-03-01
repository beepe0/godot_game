using BP.GameConsole;
using BP.ComponentSystem;
using Godot;
using Godot.Collections;
using System;
using static Godot.Projection;

public partial class MultiplayerManager : Node
{
    public static MultiplayerManager Instance { get; private set; }

    [ExportGroup("Player Scenes")]
    [Export] public PackedScene _localPlayerScene;
    [Export] public PackedScene _remotePlayerScene;

    public Dictionary<long, PlayerNetwork> Players { get; private set; } = new Dictionary<long, PlayerNetwork>();

    public override void _EnterTree()
    {
        Instance = this;
    }
    public override void _Ready()
    {
        Multiplayer.PeerConnected += (id) => InitPlayer(_remotePlayerScene, id);
    }
    public void InitPlayer(PackedScene playerScene, long id)
    {
        CharacterBody3D player = playerScene.Instantiate<CharacterBody3D>();
        AddChild(player);
        player.GetComponent<PlayerNetwork>().Create(id);
        Players.Add(id, player.GetComponent<PlayerNetwork>());
    }
    public void StartServer(string[] keys)
    {
        ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
        peer.CreateServer(ushort.Parse(keys[3]), ushort.Parse(keys[4]));
        peer.SetBindIP(keys[2]);
        //peer.CreateServer(12345, 2);
        Multiplayer.MultiplayerPeer = peer;

        Multiplayer.MultiplayerPeer.PeerConnected += (id) => GameConsole.Instance.DebugLog($"[{id}] The server connected!");
        Multiplayer.MultiplayerPeer.PeerDisconnected += (id) => GameConsole.Instance.DebugLog($"[{id}] The server disconnected!");

        GameConsole.Instance.DebugLog($"[{Multiplayer.GetUniqueId()}] The server created!");

        InitPlayer(_localPlayerScene, Multiplayer.GetUniqueId());
    }
    public void StartClient(string[] keys)
    {
        ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
        peer.CreateClient(keys[2], ushort.Parse(keys[3]));
        //peer.CreateClient("127.0.0.1", 12345);
        Multiplayer.MultiplayerPeer = peer;

        Multiplayer.MultiplayerPeer.PeerConnected += (id) => GameConsole.Instance.DebugLog($"[{id}] The client connected!");
        Multiplayer.MultiplayerPeer.PeerDisconnected += (id) => GameConsole.Instance.DebugLog($"[{id}] The client disconnected!");

        GameConsole.Instance.DebugLog($"[{Multiplayer.GetUniqueId()}] The client created!");

        InitPlayer(_localPlayerScene, Multiplayer.GetUniqueId());
    }
}
