using Assets.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Dictionary<int, PlayerManager> Players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, ItemSpawner> Spawners = new Dictionary<int, ItemSpawner>();
    public static Dictionary<int, ProjectileManager> Projectiles = new Dictionary<int, ProjectileManager>();

    public GameObject LocalPlayerPrefab;
    public GameObject PlayerPrefab;
    public GameObject ItemSpawnerPrefab;
    public GameObject ProjectilePrefab;
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

    public void CreateItemSpawner(int spawnerId, Vector3 position, bool hasItem) 
    {
        GameObject spawner = Instantiate(ItemSpawnerPrefab, position, ItemSpawnerPrefab.transform.rotation);
        spawner.GetComponent<ItemSpawner>().Initialize(spawnerId, hasItem);
        Spawners.Add(spawnerId, spawner.GetComponent<ItemSpawner>());
    }

    public void SpawnProjcetile(int id, Vector3 postion) 
    {
        GameObject projectile = Instantiate(ProjectilePrefab, postion, Quaternion.identity);
        projectile.GetComponent<ProjectileManager>().Initialize(id);
        Projectiles.Add(id, projectile.GetComponent<ProjectileManager>());
    }
}
