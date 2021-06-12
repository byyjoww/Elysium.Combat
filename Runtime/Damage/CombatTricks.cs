using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public static int Damage(IDamageDealer damageDealer, int _base, float[] _additiveMultipliers, float[] _multiplicativeMultipliers)
        {
            float add = _additiveMultipliers.Sum();
            float mult = _multiplicativeMultipliers.Aggregate((i, j) => i * j);
            float final = add * mult;

            return Mathf.CeilToInt(_base * final);
        }
    }
}