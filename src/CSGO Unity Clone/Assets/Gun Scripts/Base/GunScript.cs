using Assets.Gun_Scripts.Interfaces;
using System.Collections;
using UnityEngine;

public class GunScript : MonoBehaviour, IGunScript
{
    #region Weapon stats
    public float Damage { get; set; }
    //If object further than 100f then it cannot be hit
    public float Range { get; set; }
    //object knockback if has rigidbody
    public float ImpactForce { get; set; }
    //deafult fire rate
    public float FireRate { get; set; }
    private float NextTimeToFire = 0f;
    public bool IsAutomatedWeapon { get; set; }
    public bool IsScopedWeapon { get; set; }
    private bool isScoped = false;
    //Changed Field Of View; lower field of view = higher zoom
    public float? ScopedFOV { get; set; }
    private float NormalFOV;
    #endregion

    #region Ammo and reloading
    public int MaxAmmo { get; set; }
    private int CurrentAmmo;
    public float ReloadTime { get; set; }
    private bool IsReloading;
    public Animator Animator;
    #endregion

    public Camera FpsCam;
    public ParticleSystem MuzzleFlash;
    public GameObject ImpactEffect;

    #nullable enable
    public GameObject? ScopeOverlay;

    public GameObject WeaponCamera;



    //setting ammo to full when loaded in
    void Start()
    {
        //Start() is called the first time when the object is enabled
        CurrentAmmo = MaxAmmo;
    }

    void OnEnable() 
    {
        //OnEnable() is called everytime when the object is created
        //Stopping reloading animation if switching weapons to prevent bugs
        IsReloading = false;
        Animator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    public void Update()
    {
        Scope();
        //Getting player input
        CheckFireInputAndAmmo();
    }

    public void CheckFireInputAndAmmo()
    {

        //Check ammo count
        if (IsReloading)
            return;

        if (CurrentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        //Check if weapon is or not automatic
        switch (IsAutomatedWeapon)
        {
            //GetButtonDown activates on click
            case false:
                if (Input.GetButtonDown("Fire1") && Time.time >= NextTimeToFire)
                {
                    NextTimeToFire = Time.time + 1f / FireRate;
                    Shoot();
                }
                break;

            //GetButton activates when pressed and holded; automatic fire
            case true:
                if (Input.GetButton("Fire1") && Time.time >= NextTimeToFire)
                {
                    NextTimeToFire = Time.time + 1f / FireRate;
                    Shoot();
                }
                break;

        }

    }

    public void Shoot()
    {
        MuzzleFlash.Play();
        CurrentAmmo--;
        RaycastHit hitInfo;

        //Shooting using Ray-Casting(like in csgo, no bullet drop; laser like)
        //Check if something is hit
        if (Physics.Raycast(FpsCam.transform.position, FpsCam.transform.forward, out hitInfo, Range))
        {
            //Get name of hit object
            //Debug.Log(hitInfo.transform.name);

            Target target = hitInfo.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(Damage);
            }

            if (hitInfo.rigidbody != null)
            {
                hitInfo.rigidbody.AddForce(-hitInfo.normal * ImpactForce);
            }

            GameObject impactGo = Instantiate(ImpactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(impactGo, 2f);
        }
    }

    //Co-routing; weird syntax
    public IEnumerator Reload()
    {
        bool IsWeaponScoped = (Animator.GetBool("IsScoped"));

        IsReloading = true;
        //console log obv
        Debug.Log("Reloading...");
        //wait for reloadTime seconds
        //set Reloading bool in reloading animation
        Animator.SetBool("Reloading", true);
        //unscope for reload
        if (IsWeaponScoped) 
        {
            Animator.SetBool("IsScoped", false);
            OnUnscoped();
        }
        // -.25f to offset transition duration
        yield return new WaitForSeconds(ReloadTime - .25f);
        Animator.SetBool("Reloading", false);
        //to start shooting after the reloading animation is finished
        yield return new WaitForSeconds(.25f);
        //reload
        CurrentAmmo = MaxAmmo;

        IsReloading = false;
    }
  
    void Scope()
    {
        if(IsScopedWeapon == true) 
        {
            if (Input.GetButtonDown("Fire2"))
            {
                isScoped = !isScoped;
                Animator.SetBool("IsScoped", isScoped);

                if (isScoped) StartCoroutine(OnScoped());
                else
                    OnUnscoped();
            }
        }
    }

    void OnUnscoped()
    {
        ScopeOverlay.SetActive(false);
        WeaponCamera.SetActive(true);

        FpsCam.fieldOfView = NormalFOV;
    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds (.15f);

        ScopeOverlay.SetActive(true);
        WeaponCamera.SetActive(false);
        NormalFOV = FpsCam.fieldOfView;
        FpsCam.fieldOfView = (float)ScopedFOV;
    }

}
