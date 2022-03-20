﻿namespace Assets.Gun_Scripts
{
    public class PistolScript : GunScript
    {
        public PistolScript()
        {
            damage = 7f;
            range = 80f;
            impactForce = 50f;
            fireRate = 6f;
            isAutomaticWeapon = false;
            maxAmmo = 12;
            reloadTime = 1.5f;
        }
    }
}