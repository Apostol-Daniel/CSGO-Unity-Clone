
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Server_Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        public float Frequency = 3f;             

        public void Start() 
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

        public void ClearEnemyCountOnDisconnet() 
        {
            Enemy.Enemies.Clear();
            foreach (KeyValuePair<int, Enemy> enemy in Enemy.Enemies)
            {               
                enemy.Value.Reset();
            }
        }
    }
}
