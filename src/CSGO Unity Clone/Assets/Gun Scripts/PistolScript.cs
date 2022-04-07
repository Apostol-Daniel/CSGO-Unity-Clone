namespace Assets.Gun_Scripts
{
    public class PistolScript : GunScript
    {
        public PistolScript()
        {
            Damage = 7f;
            Range = 80f;
            ImpactForce = 50f;
            FireRate = 6f;
            IsAutomatedWeapon = false;
            MaxAmmo = 12;
            ReloadTime = 1.5f;
        }
    }
}
