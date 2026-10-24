using UnityEngine;
using Fusion;

public class NetworkManagerScript : MonoBehaviour
{
    [SerializeField] private NetworkRunner runner;

    private async void Start()
    {
        runner.ProvideInput = true;

        var sceneManager =
            runner.gameObject.AddComponent<NetworkSceneManagerDefault>();

        var result = await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = "MyBallGame",
            SceneManager = sceneManager
        });
 
    }
}
