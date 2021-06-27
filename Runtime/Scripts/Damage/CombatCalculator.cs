using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Combat
{
    public static class CombatCalculator
    {
        public static void DealDamage(IDamageDealer _applier, IElement _applierElement, IDamageable _receiver,  float[] _additiveMultipliers, float[] _multiplicativeMultipliers, bool canCrit = false)
        {
            int damage = CalculateDamage(_applier, _applierElement, _receiver, _additiveMultipliers, _multiplicativeMultipliers);
            bool crit = IsCriticalHit(_applier, damage, out damage);
            ISource source = SourceFactory.Unit(_applier, _applierElement, crit);
            _receiver.TakeDamage(damage, source);

            string critMsg = crit ? "CRITICAL HIT! " : "";
            Debug.Log($"{critMsg}Dealt {damage} {_applierElement.Name} damage to {_receiver.Element.Name} opponent");
        }

        public static bool IsCriticalHit(IDamageDealer _damageDealer, int _before, out int _after)
        {
            float critChance = 20f;
            float critDamage = 1.5f;

            float r = UnityEngine.Random.Range(0f, 100f);
            bool hasCrit = r <= critChance;

            if (hasCrit) { _damageDealer.OnCriticalHitCallback(); }
            _after = hasCrit ? Mathf.CeilToInt(_before * critDamage) : _before;
            return hasCrit;
        }

        public static int CalculateDamage(IDamageDealer damageDealer, IElement _applierElement, IDamageable _receiver, float[] _additiveMultipliers, float[] _multiplicativeMultipliers)
        {
            float defaultValue = 1f;
            _additiveMultipliers = _additiveMultipliers != null ? _additiveMultipliers : new float[0];
            _multiplicativeMultipliers = _multiplicativeMultipliers != null ? _multiplicativeMultipliers : new float[0];

            // Apply elemental multiplier
            Array.Resize(ref _multiplicativeMultipliers, _multiplicativeMultipliers.Length + 1);
            _multiplicativeMultipliers[_multiplicativeMultipliers.Length - 1] = _applierElement.Against(_receiver.Element);

            float add = _additiveMultipliers.DefaultIfEmpty(defaultValue).Sum();
            float mult = _multiplicativeMultipliers.DefaultIfEmpty(defaultValue).Aggregate((i, j) => i * j);
            float final = add * mult;
            int damage = Mathf.CeilToInt(damageDealer.Damage.Value * final);

            // Debug.Log($"Additive Multipliers: {{ {string.Join(", ", _additiveMultipliers)} }}");
            // Debug.Log($"Multiplicative Multipliers: {{ {string.Join(", ", _multiplicativeMultipliers)} }}");
            // Debug.Log($"Calculated Damage Results = Add: {add} | Mult: {mult} | Final: {final} | Base: {damageDealer.Damage.Value} | Damage: {damage}");
            
            return damage;
        }
    }
}