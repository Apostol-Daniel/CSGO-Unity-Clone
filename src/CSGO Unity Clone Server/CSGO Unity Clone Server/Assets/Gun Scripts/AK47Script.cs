namespace Assets.Gun_Scripts
{
    public class AK47Script : GunScript
    {
        public AK47Script()
        {
            Damage = 10f;
            Range = 100f;
            ImpactForce = 150f;
            FireRate = 7f; 
            IsAutomatedWeapon = true;
            MaxAmmo = 30;
            ReloadTime = 2.5f;
        }
    }
}
