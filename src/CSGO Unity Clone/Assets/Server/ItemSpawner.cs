using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public int SpawnerId;
    public bool HasItem;
    public MeshRenderer ItemModel;

    private Vector3 BasePosition;

    public void Initialize(int spawnerId, bool hasItem)
    {
        SpawnerId = spawnerId;
        HasItem = hasItem;
        ItemModel.enabled = HasItem;

        BasePosition = transform.position;
    }

    public void ItemSpawned() 
    {
        HasItem = true;
        ItemModel.enabled = true;
    }
}
