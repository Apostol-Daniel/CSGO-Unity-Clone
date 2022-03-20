using UnityEngine;

namespace Assets.Gun_Scripts
{
    public class GrenadeScript : MonoBehaviour
    {
        public float delay = 3f;
        public float radius = 5f;
        public float explosionForce = 700f;

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
            if(countdown <= 0 && !hasExploded) 
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
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            foreach(Collider nearbyObjects in colliders) 
            {
                //Add forces
                Rigidbody rb = nearbyObjects.GetComponent<Rigidbody>();
                if(rb != null) 
                {
                    rb.AddExplosionForce(explosionForce, transform.position, radius);
                }
                //Damage

            }
            
            //Remove grenade
            hasExploded = true;
            //For some reasson the explosionEffect does not get delete automatically
            Destroy(explosionEffectObject, 3.5f);
            Destroy(gameObject);
        }
    }
}
