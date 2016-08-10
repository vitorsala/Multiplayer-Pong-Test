using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameNetworkManager : NetworkManager {
    public static GameNetworkManager sharedInstance;
    private GameManager gameManager;

	void Start () {
        sharedInstance = this;

        Network.maxConnections = 2;

        Screen.SetResolution(1024, 576, false);
    }
	
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        if(startPositions.Count > 0) {
            GameObject player = Instantiate(playerPrefab);
            int index = Random.Range(0, startPositions.Count);
            player.transform.position = startPositions[index].position;
            startPositions.RemoveAt(index);

            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn) {
        base.OnServerDisconnect(conn);
    }

    public override void OnServerSceneChanged(string sceneName) {
        foreach(GameObject respawnPoint in GameObject.FindGameObjectsWithTag("Respawn")) {
            startPositions.Add(respawnPoint.transform);
        }
    }
    
}
