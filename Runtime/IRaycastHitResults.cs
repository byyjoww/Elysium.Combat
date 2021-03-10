using UnityEngine;

namespace Elysium.Combat
{
    public interface IRaycastHitResults
    {
        Vector3? Ground { get; }
        IDamageable Player { get; }
        IDamageable[] Units { get; }
    }
}