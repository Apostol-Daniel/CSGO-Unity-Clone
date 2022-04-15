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
        Instantiate(ExplosionEffect, transform.position ,Quaternion.identity);
        Destroy(gameObject);
    }
}
