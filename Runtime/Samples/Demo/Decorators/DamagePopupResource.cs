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
        private Vector3 offset = default;

        public DamagePopupResource(IResource _resource, Transform _entity, Vector3 _offset) : base(_resource)
        {
            transform = _entity.transform;
            offset = _offset;
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
            damagePopup.Create(transform.position + offset, _delta, _source.DamagePopupStyle);
        }
    }

    public static class DamagePopupResourceExtension
    {
        public static IResource WithDamagePopup(this IResource _resource, Transform _entity, Vector3 _offset)
        {
            return new DamagePopupResource(_resource, _entity, _offset);
        }
    }
}
