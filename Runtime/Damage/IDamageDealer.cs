using Elysium.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public enum DamageTeam 
    { 
        PLAYER = 0, 
        ENEMY = 1, 
    }

    public interface IDamageDealer
    {
        RefValue<int> Damage { get; set; }
        DamageTeam[] DealsDamageToTeams { get; }
        GameObject DamageDealerObject { get; }

        void CriticalHit();
    }
}
