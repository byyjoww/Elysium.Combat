using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    public static class CombatTricks
    {
        public static bool Critical(IDamageDealer _damageDealer, int _before, out int _after)
        {
            float critChance = 20f;
            float critDamage = 1.5f;

            float r = UnityEngine.Random.Range(0f, 100f);
            bool hasCrit = r <= critChance;

            if (hasCrit) { _damageDealer.CriticalHit(); }
            _after = hasCrit ? Mathf.CeilToInt(_before * critDamage) : _before;
            return hasCrit;
        }
    }
}