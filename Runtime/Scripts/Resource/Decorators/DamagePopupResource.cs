using Elysium.Utils;
using Elysium.Utils.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    [System.Serializable]
    public class DamagePopupResource : ResourceDecorator
    {
        private DamagePopup damagePopup = default;
        private Transform transform = default;

        public DamagePopupResource(IResource _resource, Transform _entity) : base(_resource)
        {
            transform = _entity.transform;
            string path = "DamagePopup";
            damagePopup = Resources.Load<DamagePopup>(path);
            if (damagePopup == null) { throw new System.Exception($"No damage popup prefab found at path {path}"); }
        }

        protected override void TriggerOnResourceGained(int _amount, ISource _source)
        {
            base.TriggerOnResourceGained(_amount, _source);
            GeneratePopup(_amount, _source);
        }

        protected override void TriggerOnResourceLost(int _amount, ISource _source)
        {
            base.TriggerOnResourceLost(_amount, _source);
            GeneratePopup(_amount, _source);
        }

        private void GeneratePopup(int _delta, ISource _source)
        {
            damagePopup.Create(transform.position, _delta, _source.DamagePopupStyle);
        }
    }

    public static class DamagePopupResourceExtension
    {
        public static IResource WithDamagePopup(this IResource _resource, Transform _entity)
        {
            return new DamagePopupResource(_resource, _entity);
        }
    }
}
