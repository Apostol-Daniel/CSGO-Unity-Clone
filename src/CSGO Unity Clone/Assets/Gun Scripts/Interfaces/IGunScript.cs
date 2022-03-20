using System.Collections;

namespace Assets.Gun_Scripts.Interfaces
{
    public interface IGunScript
    {
        void Shoot();
        IEnumerator Reload();
    }
}
