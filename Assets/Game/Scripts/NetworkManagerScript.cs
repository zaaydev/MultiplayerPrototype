using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using TMPro;

public class NetworkManagerScript : MonoBehaviour
{
[SerializeField] private NetworkRunner runner;
[SerializeField] private TMP_InputField roomNameInput;

public async void CreateRoom()
{
    await ConnectToRoom();
}

public async void JoinRoom()
{
    await ConnectToRoom();
}

private async System.Threading.Tasks.Task ConnectToRoom()
{
    string roomName = roomNameInput.text.Trim();

    if (string.IsNullOrEmpty(roomName))
    {
        Debug.LogError("Enter a room name first!");
        return;
    }

    runner.ProvideInput = true;

    var sceneManager =
        runner.gameObject.AddComponent<NetworkSceneManagerDefault>();

    var result = await runner.StartGame(new StartGameArgs
    {
        GameMode = GameMode.Shared,
        SessionName = roomName,
        SceneManager = sceneManager,
        Scene = SceneRef.FromIndex(1)
    });

    if (result.Ok)
    {
        Debug.Log("Connected to room: " + roomName);
    }
    else
    {
        Debug.LogError("Connection failed: " + result.ShutdownReason);
    }
}

}
