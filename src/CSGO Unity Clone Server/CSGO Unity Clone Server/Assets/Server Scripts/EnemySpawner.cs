
using System.Collections;
using UnityEngine;

namespace Assets.Server_Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance;

        public float Frequency = 3f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            else if (Instance != this)
            {
                Debug.Log("EnemySpawner Instance already exists, detroying object.");
                Destroy(this);
            }
        }

        public void StartSpawnCoroutine()
        {
            StartCoroutine(SpawnEnemy());
        }

        public void StopSpawnCoroutine()
        {
            StopCoroutine(SpawnEnemy());
        }

        private IEnumerator SpawnEnemy() 
        {
            yield return new WaitForSeconds(Frequency);

            if(Enemy.Enemies.Count < Enemy.MaxEnemies) 
            {
                NetworkManager.Instance.InstantiateEnemy(transform.position);
            }
            StartCoroutine(SpawnEnemy());
        }
    }
}
