namespace Assets.Gun_Scripts
{
    public class M4CarbineScript : GunScript
    {
        public M4CarbineScript()
        {
            damage = 8f;
            range = 100f;
            impactForce = 100f;
            fireRate = 8f;
            isAutomaticWeapon = true;
            maxAmmo = 30;
            reloadTime = 2.5f;
        }
    }
}
