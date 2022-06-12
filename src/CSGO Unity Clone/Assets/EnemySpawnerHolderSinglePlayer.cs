using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerHolderSinglePlayer : MonoBehaviour
{
    public static EnemySpawnerHolderSinglePlayer EnemySpawnerHolderMultiplayerInstance;

    private void Awake()
    {

        if (EnemySpawnerHolderMultiplayerInstance == null)
        {
            EnemySpawnerHolderMultiplayerInstance = this;
        }

        else if (EnemySpawnerHolderMultiplayerInstance != this)
        {
            Debug.Log("EnemySpawnerHolderInstance already exists, detroying object.");
            Destroy(this);
        }
    }

    public static EnemySpawnerHolderSinglePlayer Instance()
    {
        return EnemySpawnerHolderMultiplayerInstance;
    }

    public void StartSpawnCoroutine()
    {
        foreach (Transform enemySpawner in transform)
        {
            var enemySpawnerScript = enemySpawner.GetComponent<EnemySpawnerSinglePlayer>();
            enemySpawnerScript.StartSpawnCoroutine();
        }
    }


    public void ClearEnemyCountOnDisconnet()
    {
        foreach (Transform enemySpawner in transform)
        {
            var enemySpawnerScript = enemySpawner.GetComponent<EnemySpawnerSinglePlayer>();
            enemySpawnerScript.StopSpawnCoroutine();
            enemySpawnerScript.ClearEnemyCountOnDisconnet();
        }
    }
}
