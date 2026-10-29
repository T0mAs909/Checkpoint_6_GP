using Fusion;
using UnityEngine;

public class MazeSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] NetworkObject playerPrefab;
    [SerializeField] NetworkObject matchPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player != Runner.LocalPlayer) return;
        bool isMaster = Runner.IsSharedModeMasterClient;

        if (isMaster)
        {
            Runner.Spawn(matchPrefab, Vector3.zero, Quaternion.identity);
        }

        Vector3 spawnPosition = isMaster ? new Vector3(-12, 5, -37) : new Vector3(12, 5, -37);
        Quaternion spawnRotation = Quaternion.Euler(0, 90, 0);
        NetworkObject playerObjct = Runner.Spawn(playerPrefab, spawnPosition, spawnRotation);
        Runner.SetPlayerObject(player, playerObjct);
    }
}