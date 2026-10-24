
using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private NetworkObject playerGreenPrefab;
    [SerializeField] private NetworkObject playerRedPrefab;

    public void PlayerJoined(PlayerRef player)
    {
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

        Runner.Spawn(
            selectedPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }

}
