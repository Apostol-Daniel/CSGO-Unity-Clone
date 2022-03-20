namespace Assets.Gun_Scripts
{
    public class AK47Script : GunScript
    {
        public AK47Script()
        {
            damage = 10f;
            range = 100f;
            impactForce = 150f;
            fireRate = 7f; 
            isAutomaticWeapon = true;
            maxAmmo = 30;
            reloadTime = 2.5f;
        }
    }
}
