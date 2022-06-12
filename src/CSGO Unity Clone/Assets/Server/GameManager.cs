using Assets.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GameManagerInstance;
    public static Dictionary<int, PlayerManager> Players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, ItemSpawner> Spawners = new Dictionary<int, ItemSpawner>();
    public static Dictionary<int, ProjectileManager> Projectiles = new Dictionary<int, ProjectileManager>();
    public static Dictionary<int, EnemyManager> Enemies = new Dictionary<int, EnemyManager>();

    public GameObject LocalPlayerPrefab;
    public GameObject PlayerPrefab;
    public GameObject GrenadeSpawnerPrefab;
    public GameObject ProjectilePrefab;
    public GameObject EnemyPrefab;
    private void Awake()
    {
        if (GameManagerInstance == null)
        {
            GameManagerInstance = this;
        }

        else if (GameManagerInstance != this)
        {
            Debug.Log("Instance already exists, detroying object.");
            Destroy(this);
        }
    }

    public void Reset()
    {
        
    }

    public static GameManager Instance() 
    {
        return GameManagerInstance;
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
        GameObject spawner = Instantiate(GrenadeSpawnerPrefab, position, GrenadeSpawnerPrefab.transform.rotation);
        spawner.GetComponent<ItemSpawner>().Initialize(spawnerId, hasItem);
        Spawners.Add(spawnerId, spawner.GetComponent<ItemSpawner>());
    }

    public void SpawnProjectile(int id, Vector3 postion) 
    {
        GameObject projectile = Instantiate(ProjectilePrefab, postion, Quaternion.identity);
        projectile.GetComponent<ProjectileManager>().Initialize(id);
        Projectiles.Add(id, projectile.GetComponent<ProjectileManager>());
    }

    public void SpawnEnemy(int id, Vector3 position) 
    {
        GameObject enemy = Instantiate(EnemyPrefab, position, Quaternion.identity);
        enemy.GetComponent<EnemyManager>().Initialize(id);
        Enemies.Add(id, enemy.GetComponent<EnemyManager>());
    }

    public void ClearData() 
    {
        Players.Clear();
        Spawners.Clear();
        Projectiles.Clear();
        Enemies.Clear();
    }
}
