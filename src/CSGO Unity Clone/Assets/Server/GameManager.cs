using Assets.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Dictionary<int, PlayerManager> Players = new Dictionary<int, PlayerManager>();

    public GameObject LocalPlayerPrefab;
    public GameObject PlayerPrefab;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Debug.Log("Instance already exists, detroying object.");
            Destroy(this);
        }
    }

    public void SpawnPlayer(int playerId, string userName, Vector3 position, Quaternion rotation) 
    {
        GameObject player;
        if (playerId == Client.ClientInstance.ClientId)
        {
            player = Instantiate(LocalPlayerPrefab, position, rotation);

        }
        else 
        {
            player = Instantiate(PlayerPrefab, position, rotation);
        }

        player.GetComponent<PlayerManager>().Initialize(playerId, userName);
        Players.Add(playerId, player.GetComponent<PlayerManager>());
    }
}
