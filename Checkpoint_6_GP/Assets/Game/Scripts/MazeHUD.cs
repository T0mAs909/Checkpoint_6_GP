using Fusion;
using System;
using TMPro;
using UnityEngine;

public class MazeHUD : MonoBehaviour
{
    [SerializeField] NetworkRunner runner;
    [SerializeField] TMP_Text textMatch;
    [SerializeField] GameObject resultsPanel;
    [SerializeField] TMP_Text textResult;

    void Start()
    {
        resultsPanel.SetActive(false);
        textMatch.text = "Aguardando estado da partida...";
    }

    void Update()
    {
        if (runner == null || !runner.IsRunning) return;
        MazeMatch match = MazeMatch.Instance;

        if(match == null)
        {
            textMatch.text = "Aguardando estado da partida...";
            return;
        }

        if (match.IsOver)
        {
            ShowResult(match);
        }
        else
        {
            resultsPanel.SetActive(false);
            textMatch.text = match.CanPlay ? "Jogo em andamento" : "Aguardando segundo jogador...";
        }
    }

    private void ShowResult(MazeMatch match)
    {
        resultsPanel.SetActive(true);
        textMatch.text = "Partida encerrada";
        bool localPlayerWon = match.Winner == runner.LocalPlayer;
        string winnerName = match.Winner == match.GreenPlayer ? "Verde" : "Vermelho";
        string message = localPlayerWon ? "Você perdeu" : "Você venceu";
        textResult.text = message + "\n Vencedor: Jogador " + winnerName;
    }
}