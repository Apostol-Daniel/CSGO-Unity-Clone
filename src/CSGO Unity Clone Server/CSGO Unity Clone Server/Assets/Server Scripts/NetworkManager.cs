using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    public GameObject PlayerPrefab;
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

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 128;      

        Server.Start(10, 26950);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer() 
    {
        return Instantiate(PlayerPrefab, new Vector3(0f, 1.5f, 0f), Quaternion.identity).GetComponent<Player>();
    }

    public Projectile InstantiateProjectile(Transform shootOrigin) 
    {
        return Instantiate(ProjectilePrefab, shootOrigin.position + shootOrigin.forward * 0.7f, Quaternion.identity).GetComponent<Projectile>();
    }
}
