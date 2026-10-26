using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class NetworkManagerScript : MonoBehaviour
{
    [Header("Network")]
    [SerializeField] private NetworkRunner runner;
    [SerializeField] private TMP_InputField roomNameInput;

    [Header("Buttons")]
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;

    [SerializeField] private TMP_Text createButtonLabel;
    [SerializeField] private TMP_Text joinButtonLabel;

    [Header("Status")]
    [SerializeField] private TMP_Text statusText;

    private bool isConnecting;

    private string originalCreateLabel;
    private string originalJoinLabel;

    private void Awake()
    {
        if (createButtonLabel != null)
            originalCreateLabel = createButtonLabel.text;

        if (joinButtonLabel != null)
            originalJoinLabel = joinButtonLabel.text;
    }

    public async void CreateRoom()
    {
        await ConnectToRoom();
    }

    public async void JoinRoom()
    {
        await ConnectToRoom();
    }

    private async Task ConnectToRoom()
    {
        // Prevent repeated clicks.
        if (isConnecting)
            return;

        if (runner == null || roomNameInput == null)
        {
            Debug.LogError("NetworkRunner or Room Name Input is missing!");
            return;
        }

        string roomName = roomNameInput.text.Trim();

        if (string.IsNullOrEmpty(roomName))
        {
            if (statusText != null)
                statusText.text = "Enter a room name first!";

            return;
        }

        isConnecting = true;
        SetBusy(true);

        if (statusText != null)
            statusText.text = "Connecting to room...";

        try
        {
            runner.ProvideInput = true;

            // Avoid adding duplicate scene managers.
            NetworkSceneManagerDefault sceneManager =
                runner.GetComponent<NetworkSceneManagerDefault>();

            if (sceneManager == null)
            {
                sceneManager =
                    runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
            }

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

                if (statusText != null)
                    statusText.text = "Connected! Loading game...";

                // Keep the controls disabled while entering the game.
            }
            else
            {
                Debug.LogError(
                    "Connection failed: " + result.ShutdownReason
                );

                if (statusText != null)
                    statusText.text =
                        "Connection failed: " + result.ShutdownReason;

                isConnecting = false;
                SetBusy(false);
            }
        }
        catch (System.Exception exception)
        {
            Debug.LogException(exception);

            if (statusText != null)
                statusText.text = "Connection failed. Try again.";

            isConnecting = false;
            SetBusy(false);
        }
    }

    private void SetBusy(bool busy)
    {
        if (createRoomButton != null)
            createRoomButton.interactable = !busy;

        if (joinRoomButton != null)
            joinRoomButton.interactable = !busy;

        if (roomNameInput != null)
            roomNameInput.interactable = !busy;

        if (createButtonLabel != null)
            createButtonLabel.text = busy
                ? "Connecting..."
                : originalCreateLabel;

        if (joinButtonLabel != null)
            joinButtonLabel.text = busy
                ? "Connecting..."
                : originalJoinLabel;
    }
}