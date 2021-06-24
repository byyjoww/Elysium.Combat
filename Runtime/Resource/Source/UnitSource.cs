using System;

namespace Elysium.Combat
{
    public static partial class ResourceModifierSourceFactory
    {
        private class UnitSource : ISource
        {
            public IDamageDealer DamageDealer { get; private set; }
            public IElement Element { get; private set; }
            public DamagePopupStyle DamagePopupStyle { get; private set; }

            public UnitSource(IDamageDealer _damageDealer, IElement _element, DamagePopupStyle _style)
            {
                this.DamageDealer = _damageDealer;
                this.Element = _element;
                this.DamagePopupStyle = _style;
            }
        }        
    }
}