using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    public interface IElement
    {
        string Name { get; }

        float Against(IElement _element);
        DamagePopupStyle GetStyle(bool crit);
    }
}