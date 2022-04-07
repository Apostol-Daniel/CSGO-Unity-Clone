namespace Assets.Gun_Scripts
{
    public class M4CarbineScript : GunScript
    {
        public M4CarbineScript()
        {
            Damage = 8f;
            Range = 100f;
            ImpactForce = 100f;
            FireRate = 8f;
            IsAutomatedWeapon = true;
            MaxAmmo = 30;
            ReloadTime = 2.5f;
        }
    }
}
