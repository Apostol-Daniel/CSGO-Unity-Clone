using System.Collections;
using UnityEngine;

namespace Assets.Gun_Scripts
{
    public class AWPScript : GunScript
    {        

        public AWPScript()
        {
            Damage = 100f;
            Range = 200f;
            ImpactForce = 250f;
            FireRate = .5f;
            IsAutomatedWeapon = false;
            IsScopedWeapon = true;
            ScopedFOV = 15f;
            MaxAmmo = 5;
            ReloadTime = 3.5f;
        }          
    }
}
