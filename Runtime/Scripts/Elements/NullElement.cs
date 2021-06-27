using UnityEngine;

namespace Elysium.Combat
{
    public class NullElement : IElement
    {
        public string Name => default;

        public float Against(IElement _element)
        {
            return 1f;
        }

        public DamagePopupStyle GetStyle(bool crit)
        {
            return DamagePopupStyle.Default;
        }
    }
}