using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    public interface IElement
    {
        DamagePopupStyle GetStyle(bool crit);
    }
}