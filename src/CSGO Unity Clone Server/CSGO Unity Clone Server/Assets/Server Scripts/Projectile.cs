using Assets.Server_Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Dictionary<int, Projectile> Projectiles = new Dictionary<int, Projectile>();
    private static int NextProjectileId = 1;


    public int Id;
    public Rigidbody Rigidbody;
    public int PlayerId;
    public Vector3 Force;
    public float ExplosionRadius = 1.5f;
    public float ExplosionDamage = 75f;


    private void Start()
    {
        Id = NextProjectileId;
        NextProjectileId++;
        Projectiles.Add(Id, this);

        ServerSend.SpawnProjectile(this, PlayerId);

        Rigidbody.AddForce(Force, ForceMode.VelocityChange);
        StartCoroutine(ExplodeAfterTime());
    }

    private void FixedUpdate()
    {
        ServerSend.ProjectilePosition(this);
    }

    //Use with another grenade ?
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Explode();
    //}

    public void Initialize(Vector3 throwingDirection, float ThrowForce, int playerId)
    {
        Force = throwingDirection * ThrowForce;
        PlayerId = playerId;
    }

    private void Explode() 
    {       
        ServerSend.ProjectileExploded(this);

        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach(Collider collider in colliders) 
        {
            if (collider.CompareTag("Player")) 
            {
                collider.GetComponentInParent<Player>().TakeDamage(ExplosionDamage);
            }
            else if (collider.CompareTag("Enemy")) 
            {
                collider.GetComponent<Enemy>().TakeDamage(ExplosionDamage);
            }
        }

        Projectiles.Remove(Id);
        Destroy(gameObject);
    }

    private IEnumerator ExplodeAfterTime() 
    {
        yield return new WaitForSeconds(3f);

        Explode();
    }
}
