using System.Collections;
using UnityEngine;

namespace Assets.Gun_Scripts
{
    public class AWPScript : GunScript
    {
        private bool isScoped = false;
        public float? ScopedFOV = 15f;
        private float NormalFOV;
        public GameObject ScopeOverlay;

        public AWPScript()
        {
            Damage = 100f;
            Range = 200f;
            ImpactForce = 250f;
            FireRate = .5f;
            IsAutomatedWeapon = false;
            MaxAmmo = 5;
            ReloadTime = 3.5f;
        }

        public new void Update()
        {
            Scope();
            base.Update();
        }

        void Scope()
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

        void OnUnscoped()
        {
            ScopeOverlay.SetActive(false);
            WeaponCamera.SetActive(true);

            FpsCam.fieldOfView = NormalFOV;
        }

        IEnumerator OnScoped()
        {
            yield return new WaitForSeconds(.15f);

            ScopeOverlay.SetActive(true);
            WeaponCamera.SetActive(false);
            NormalFOV = FpsCam.fieldOfView;
            FpsCam.fieldOfView = (float)ScopedFOV;
        }
    }
}
