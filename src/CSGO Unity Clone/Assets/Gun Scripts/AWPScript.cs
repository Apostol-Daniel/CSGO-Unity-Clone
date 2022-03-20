namespace Assets.Gun_Scripts
{
    public class AWPScript : GunScript
    {
        public AWPScript()
        {
            damage = 100f;
            range = 200f;
            impactForce = 250f;
            fireRate = .5f;
            isAutomaticWeapon = false;
            maxAmmo = 5;
            reloadTime = 3.5f;
        }
    }
}
