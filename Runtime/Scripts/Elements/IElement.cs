using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    public interface IElement
    {
        Element Name { get; }

        float Against(IElement _element);
        DamagePopupStyle GetStyle(bool crit);
    }
}