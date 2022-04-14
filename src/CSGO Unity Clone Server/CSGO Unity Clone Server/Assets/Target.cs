
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;

    public void TakeDamage( float damage) 
    {
        health -= damage;
        if(health <= 0f) 
        {
            Die();
        }

        void Die() 
        {
            Destroy(gameObject);
        }
    }
}
