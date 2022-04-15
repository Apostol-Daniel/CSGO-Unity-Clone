using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public int SpawnerId;
    public bool HasItem;
    public MeshRenderer ItemModel;

    public float ItemRotationSpeed = 50f;
    public float ItemBobSpeed = 2f;
    private Vector3 BasePosition;

    private void Update()
    {
        if (HasItem) 
        {
            transform.Rotate(Vector3.up, ItemRotationSpeed * Time.deltaTime, Space.World);
            transform.position = BasePosition + new Vector3(0f, 0.25f * Mathf.Sin(Time.time * ItemBobSpeed), 0f);
        }
    }

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

    public void ItemPickedUp() 
    {
        HasItem = false;
        ItemModel.enabled = false;
    }
}
