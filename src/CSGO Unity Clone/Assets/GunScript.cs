using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    //Don't need start, only update
    public float damage = 10f;
    //If object further than 100f then it cannot be hit
    public float range = 100f;
    //object knockback if has rigidbody
    public float impactForce = 150f;
    //deafult fire rate
    public float fireRate = 15f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    // Update is called once per frame
    void Update()
    {
        //Getting player input
        //GetButtonDown activates on click
        //GetButton activates when pressed and holded; automatic fire
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            //the greater the fireRate, the lower is nextTimeToFire
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        //Shooting using Ray-Casting(like in csgo, no bullet drop; laser like)
        void Shoot() 
        {
            muzzleFlash.Play();
            RaycastHit hitInfo;

            //Check if something is hit
            if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, range)) 
            {
                Debug.Log(hitInfo.transform.name);

                Target target = hitInfo.transform.GetComponent<Target>();
                if(target != null) 
                {
                    target.TakeDamage(damage);
                }

                if(hitInfo.rigidbody != null) 
                {
                    hitInfo.rigidbody.AddForce(-hitInfo.normal * impactForce);
                }

                GameObject impactGo = Instantiate(impactEffect,hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGo, 2f);
            }
        }
    }
}
