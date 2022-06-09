using Assets.Server_Scripts;
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

    void ClearEnemyCountOnDisconnet() 
    {
        foreach(Transform enemySpawner in transform) 
        {
            var enemySpawnerScript = enemySpawner.GetComponent<EnemySpawner>();
            enemySpawnerScript.ClearEnemyCountOnDisconnet();
        }
    }
}
