using Elysium.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    public enum DamageTeam { PLAYER = 0, ENEMY = 1, }

    public interface IDamageDealer
    {
        RefValue<int> Damage { get; set; }
        List<DamageTeam> DealsDamageToTeams { get; }
        GameObject DamageDealerObject { get; }
        // event Action<IDamageable> OnAttack;
    }
}
