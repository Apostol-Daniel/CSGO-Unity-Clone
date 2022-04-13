using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

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

    private void Start()
    {
        #if UNITY_EDITOR
        Debug.Log("Build the project to start the server.");
        #else
        Server.Start(10, 26950);
        #endif
    }

    public Player InstantiatePlayer() 
    {
        return Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
    }
}
