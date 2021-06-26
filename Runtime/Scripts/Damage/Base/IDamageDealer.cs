using Elysium.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public interface IDamageDealer
    {
        RefValue<int> Damage { get; set; }
        DamageTeam[] DealsDamageToTeams { get; }

        void OnCriticalHitCallback();
    }
}
