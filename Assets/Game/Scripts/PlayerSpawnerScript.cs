using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private NetworkObject playerGreenPrefab;
    [SerializeField] private NetworkObject playerRedPrefab;

    private bool isRegistered;

    private void Start()
    {
        RegisterSpawner();
    }

    private void RegisterSpawner()
    {
        NetworkRunner runner = NetworkRunner.GetRunnerForGameObject(gameObject);

        if (runner == null || !runner.IsRunning)
        {
            Debug.LogError("PlayerSpawner: No running NetworkRunner found!");
            return;
        }

        runner.AddGlobal(this);
        isRegistered = true;

        Debug.Log("PlayerSpawner registered with Fusion.");
    }

    private void OnDestroy()
    {
        if (!isRegistered)
            return;

        NetworkRunner runner = NetworkRunner.GetRunnerForGameObject(gameObject);

        if (runner != null && runner.IsRunning)
            runner.RemoveGlobal(this);
    }

    public void PlayerJoined(PlayerRef player)
    {
        Debug.Log($"Player joined: {player}");

        if (player != Runner.LocalPlayer)
            return;

        int playerSlot = player.PlayerId % 2;

        NetworkObject selectedPrefab;
        Vector3 spawnPosition;

        if (playerSlot == 0)
        {
            selectedPrefab = playerGreenPrefab;
            spawnPosition = new Vector3(-2f, 1f, 0f);
        }
        else
        {
            selectedPrefab = playerRedPrefab;
            spawnPosition = new Vector3(2f, 1f, 0f);
        }

        Runner.Spawn(selectedPrefab, spawnPosition, Quaternion.identity);

        Debug.Log("Local player spawned.");
    }

}
