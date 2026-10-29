using Fusion;
using UnityEngine;

public class MazeMatch : NetworkBehaviour
{
    public static MazeMatch Instance {  get; private set; }

    [Networked]
    public NetworkBool IsOver { get; set; }

    [Networked]
    public PlayerRef Winner { get; set; }

    [Networked]
    public PlayerRef GreenPlayer { get; set; }

    public bool CanPlay { get
        {
            if (IsOver) return false;
            int connectedPlayer = 0;
            foreach(PlayerRef player in Runner.ActivePlayers)
            {
                connectedPlayer++;
            }
            return connectedPlayer == 2;
        }   
    }

    public override void Spawned()
    {
        Instance = this;

        if (HasStateAuthority)
        {
            IsOver = false;
            GreenPlayer = Runner.LocalPlayer;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]

    public void RPC_ReportVictory(PlayerRef won, RpcInfo info = default)
    {
        if (!HasStateAuthority || IsOver) return;
        if (!CanPlay) return;
        if (info.Source != won) return;

        foreach (PlayerRef player in Runner.ActivePlayers)
        {
            if (player == won) continue;

            Winner = player;
            IsOver = true;

            Debug.Log("Partida encerrada");
            break;
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (Instance != this)
        {
            Instance = null;
        }
    }
}