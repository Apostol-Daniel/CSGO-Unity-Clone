namespace Assets.Gun_Scripts
{
    public class PistolScript : GunScript
    {
        public PistolScript()
        {
            damage = 7f;
            range = 80f;
            impactForce = 50f;
            fireRate = 5f;
            isAutomaticWeapon = false;
        }
    }
}
