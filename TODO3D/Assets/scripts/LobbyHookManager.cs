using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyHookManager : LobbyHook {

    // Use this for initialization
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        PlayerScript2 p = gamePlayer.GetComponent<PlayerScript2>();

        p.playerName = lobby.playerName;
        p.color = new PlayerScript2.PlayerColor(lobby.playerColor.r, lobby.playerColor.g, lobby.playerColor.b);
    }
}
