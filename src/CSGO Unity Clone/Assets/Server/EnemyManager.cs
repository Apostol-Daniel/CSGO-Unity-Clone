

using UnityEngine;

namespace Assets.Server
{
    public class EnemyManager : MonoBehaviour
    {
        public int Id;
        public float Health;
        public float MaxHealth = 100f;

        public void Initialize(int id)
        {
            Id = id;
            Health = MaxHealth;
        }

        public void SetHealth(float health) 
        {
            Health = health;

            if(Health <= 0f) 
            {
                Destroy(gameObject);
            }
        }
    }
}
