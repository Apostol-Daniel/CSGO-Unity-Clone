
namespace Assets.Gun_Scripts
{
    public class KnifeScript : GunScript
    {
        public KnifeScript()
        {
            Damage = 25f;
            Range = 3f;
            ImpactForce = 50f;
            FireRate = 1.5f;
            IsAutomatedWeapon = true;
            MaxAmmo = 10;
            ReloadTime = 0f;
        }
    }
}
