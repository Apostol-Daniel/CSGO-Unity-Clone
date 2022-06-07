
using System.Collections;
using UnityEngine;

namespace Assets.Server_Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        public float Frequency = 3f;

        private void Start()
        {
            StartCoroutine(SpawnEnemy());
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
