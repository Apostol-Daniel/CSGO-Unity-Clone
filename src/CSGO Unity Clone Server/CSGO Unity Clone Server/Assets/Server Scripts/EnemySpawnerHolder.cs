using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerHolder : MonoBehaviour
{
    public static EnemySpawnerHolder EnemySpawnerHolderInstance;

    private void Awake()
    {       

        if (EnemySpawnerHolderInstance == null)
        {
            EnemySpawnerHolderInstance = this;
        }

        else if (EnemySpawnerHolderInstance != this)
        {
            Debug.Log("EnemySpawnerHolderInstance already exists, detroying object.");
            Destroy(this);
        }
    }
    public static EnemySpawnerHolder Instance()
    {
        return EnemySpawnerHolderInstance;
    }
}
