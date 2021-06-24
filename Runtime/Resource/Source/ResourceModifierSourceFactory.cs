using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    public static partial class ResourceModifierSourceFactory
    {
        public static ISource System => new SystemSource();
        public static ISource Cost => new CostSource();
        public static ISource Unit(IDamageDealer _damageDealer, IElement _element, bool _criticalHit = false)
        {
            var style = _element.GetStyle(_criticalHit);
            return new UnitSource(_damageDealer, _element, style);
        }
    }
}