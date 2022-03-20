using Assets.Gun_Scripts.Interfaces;
using System.Collections;
using UnityEngine;

public class GunScript : MonoBehaviour, IGunScript
{
    #region Weapon stats
    public float damage { get; set; }
    //If object further than 100f then it cannot be hit
    public float range { get; set; }
    //object knockback if has rigidbody
    public float impactForce { get; set; }
    //deafult fire rate
    public float fireRate { get; set; }
    private float nextTimeToFire = 0f;
    public bool isAutomaticWeapon { get; set; }
    #endregion

    #region Ammo and reloading
    public int maxAmmo { get; set; }
    private int currentAmmo;
    public float reloadTime { get; set; }
    private bool isReloading;
    public Animator reloadingAnimation;
    #endregion

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;



    //setting ammo to full when loaded in
    void Start()
    {
        //Start() is called the first time when the object is enabled
        currentAmmo = maxAmmo;
    }

    void OnEnable() 
    {
        //OnEnable() is called everytime when the object is created
        //Stopping reloading animation if switching weapons to prevent bugs
        isReloading = false;
        reloadingAnimation.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {
        //Getting player input
        CheckFireInputAndAmmo();
    }

    public void CheckFireInputAndAmmo()
    {

        //Check ammo count
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        //Check if weapon is or not automatic
        switch (isAutomaticWeapon)
        {
            //GetButtonDown activates on click
            case false:
                if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
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

    public void Shoot()
    {
        muzzleFlash.Play();
        currentAmmo--;
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

    //Co-routing; weird syntax
    public IEnumerator Reload()
    {
        isReloading = true;
        //console log obv
        Debug.Log("Reloading...");
        //wait for reloadTime seconds

        //set Reloading bool in reloading animation
        reloadingAnimation.SetBool("Reloading", true);
        // -.25f to offset transition duration
        yield return new WaitForSeconds(reloadTime - .25f);
        reloadingAnimation.SetBool("Reloading", false);
        //to start shooting after the reloading animation is finished
        yield return new WaitForSeconds(.25f);
        //reload
        currentAmmo = maxAmmo;

        isReloading = false;
    }
}
