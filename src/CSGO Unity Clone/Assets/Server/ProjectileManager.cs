using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public int Id;
    public GameObject ExplosionEffect;

    public void Initialize(int id)
    {
        Id = id;
    }

    public void Explode(Vector3 position) 
    {
        transform.position = position;        
        GameObject explosionEffectObject = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        GameManager.Projectiles.Remove(Id);
        Destroy(explosionEffectObject, 3.5f);
        Destroy(gameObject);
    }
}
