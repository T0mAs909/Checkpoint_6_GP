using Fusion;
using TMPro;
using UnityEngine;

public class MazeLauncher : MonoBehaviour
{
    [SerializeField] NetworkRunner runner;
    [SerializeField] TMP_InputField inputRoom;
    [SerializeField] TMP_Text textConnection;
    [SerializeField] GameObject menuPanel;

    bool connecting;

    public async void ConnectToRoom()
    {
        if (connecting || runner.IsRunning) return;

        string roomName = inputRoom.text;

        if (string.IsNullOrEmpty(roomName))
        {
            textConnection.text = "Digite o nome da sala";
            return;
        }

        connecting = true;
        textConnection.text = "Conectando...";
        runner.ProvideInput = false;
        var sceneManager = GetComponent<NetworkSceneManagerDefault>();

        var result = await runner.StartGame(
            new StartGameArgs
            {
                GameMode = GameMode.Shared,
                SessionName = roomName,
                PlayerCount = 2,
                SceneManager = sceneManager,

            }
        );

        if (result.Ok)
        {
            menuPanel.SetActive(false);
        }
        else
        {
            textConnection.text = "Erro: " + result.ShutdownReason;
            Debug.Log(result.ShutdownReason);
        }

        connecting = false;
    }
}