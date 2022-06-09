using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static Dictionary<int, ItemSpawner> Spawners = new Dictionary<int, ItemSpawner>();

    private static int NextSpwanerId = 1;
    public bool IsSpawningAllowed = false;

    public int SpawnerId;
    public bool HasItem = false;  

    private void Start()
    {      
        HasItem = false;
        SpawnerId = NextSpwanerId;
        NextSpwanerId++;
        Spawners.Add(SpawnerId, this);

        StartCoroutine(SpawnItem());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HasItem && other.CompareTag("Player")) 
        {
            Player player = other.GetComponentInParent<Player>();
            if (player.AttemptPickUpItem()) 
            {
                ItemPickUp(player.Id);
            }
        }
    }

    private IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(10f);

        HasItem = true;

        ServerSend.ItemSpawned(SpawnerId);
    }

    private void ItemPickUp(int playerId) 
    {
        HasItem = false;
        ServerSend.ItemPickedUp(SpawnerId, playerId);

        StartCoroutine(SpawnItem());
    }

}

