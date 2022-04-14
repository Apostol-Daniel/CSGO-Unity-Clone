using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Gun_Scripts.Interfaces
{
    public interface IScopeScript
    {
        void Scope();
        IEnumerator OnScoped();
        void OnUnscoped();
        void ExitScopeAnimationIfWeaponIsScoped();
    }
}
