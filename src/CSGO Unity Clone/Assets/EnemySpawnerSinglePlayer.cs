using Assets.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerSinglePlayer : MonoBehaviour
{
    public float Frequency = 3f;
    public GameObject EnemyPrefab;


    public void StartSpawnCoroutine()
    {
        if(Client.Instance().IsConnected) Destroy(this);

        StartCoroutine(SpawnEnemy());
    }

    public void StopSpawnCoroutine()
    {
        StopCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(Frequency);

        if (EnemySinglePlayer.Enemies.Count < EnemySinglePlayer.MaxEnemies)
        {
            Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
        }
        StartCoroutine(SpawnEnemy());
    }

    public void ClearEnemyCountOnDisconnet()
    {
        EnemySinglePlayer.Enemies.Clear();
        foreach (KeyValuePair<int, EnemySinglePlayer> enemy in EnemySinglePlayer.Enemies)
        {
            enemy.Value.Reset();
        }
    }
}
