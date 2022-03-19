using Assets.Gun_Scripts.Interfaces;
using UnityEngine;

public class GunScript : MonoBehaviour, IGunScript
{
    //Don't need start, only update
    public float damage { get; set; }
    //If object further than 100f then it cannot be hit
    public float range { get; set; }
    //object knockback if has rigidbody
    public float impactForce { get; set; }
    //deafult fire rate
    public float fireRate { get; set; }
    public bool isAutomaticWeapon { get; set; }

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    // Update is called once per frame
    void Update()
    {
        //Getting player input
        CheckFireInput();                   
    }

    public void Shoot()
    {
        muzzleFlash.Play();
        RaycastHit hitInfo;

        //Shooting using Ray-Casting(like in csgo, no bullet drop; laser like)
        //Check if something is hit
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hitInfo, range))
        {
            Debug.Log(hitInfo.transform.name);

            Target target = hitInfo.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hitInfo.rigidbody != null)
            {
                hitInfo.rigidbody.AddForce(-hitInfo.normal * impactForce);
            }

            GameObject impactGo = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(impactGo, 2f);
        }
    }

    public void CheckFireInput() 
    {
        switch (isAutomaticWeapon) 
        {
            //GetButtonDown activates on click
            case false:
                if(Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
                {                
                    nextTimeToFire = Time.time + 1f / fireRate;
                    Shoot();
                }
                break;

            //GetButton activates when pressed and holded; automatic fire
            case true:
                if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1f / fireRate;
                    Shoot();
                }
                break;

        }
        
    }
}
