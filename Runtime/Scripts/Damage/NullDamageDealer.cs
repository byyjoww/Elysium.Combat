using Elysium.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    public class NullDamageDealer : IDamageDealer
    {
        public RefValue<int> Damage { get; set; } = new RefValue<int>(() => 0);

        public DamageTeam[] DealsDamageToTeams => new DamageTeam[0];

        public void OnCriticalHitCallback()
        {
            
        }
    }
}