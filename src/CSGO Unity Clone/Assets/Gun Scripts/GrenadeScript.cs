using UnityEngine;

namespace Assets.Gun_Scripts
{
    public class GrenadeScript : MonoBehaviour
    {
        public float delay = 3f;
        public float radius = 5f;
        public float explosionForce = 200f;

        public GameObject explosionEffect;

        float countdown;
        bool hasExploded;

        void Start()
        {
            countdown = delay;
        }

        void Update()
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0 && !hasExploded)
            {
                Explode();
            }
        }

        void Explode()
        {
            //Show explosion effect
            GameObject explosionEffectObject = Instantiate(explosionEffect, transform.position, transform.rotation);
            Debug.Log("Boom");

            //Get nearby objects
            Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider nearbyObjects in collidersToDestroy)
            {
                //Damage
                //First destroy object -> create more objects(pieces of objects)
                Destructible dest = nearbyObjects.GetComponent<Destructible>();
                EnemySinglePlayer enemySinglePlayer = nearbyObjects.GetComponentInParent<EnemySinglePlayer>();

                if (dest != null)
                {
                    dest.Destroy();
                }

                if(enemySinglePlayer != null) 
                {
                    enemySinglePlayer.TakeDamage(explosionForce);
                }

            }

            Collider[] collidersToMove = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider nearbyObjects in collidersToMove)
            {
                //Add forces to the pieces of the objects
                Rigidbody rb = nearbyObjects.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, radius);
                }
            }

            //Remove grenade
            hasExploded = true;
            //For some reasson the explosionEffect does not get delete automatically
            Destroy(explosionEffectObject, 3.5f);
            Destroy(gameObject);
        }
    }
}
