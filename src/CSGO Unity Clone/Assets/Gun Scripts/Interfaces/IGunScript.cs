using System.Collections;

namespace Assets.Gun_Scripts.Interfaces
{
    public interface IGunScript : IScopeScript
    {
        void Shoot();
        IEnumerator Reload();
        void Update();
    }
}
