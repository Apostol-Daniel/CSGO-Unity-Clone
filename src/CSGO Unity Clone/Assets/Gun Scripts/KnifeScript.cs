
namespace Assets.Gun_Scripts
{
    public class KnifeScript : GunScript
    {
        public KnifeScript()
        {
            damage = 25f;
            range = 3f;
            impactForce = 50f;
            fireRate = 1.5f;
            isAutomaticWeapon = true;
            maxAmmo = 10;
            reloadTime = 0f;
        }
    }
}
