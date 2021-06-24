namespace Elysium.Combat
{
    public static partial class ResourceModifierSourceFactory
    {
        private class SystemSource : ISource
        {
            public IDamageDealer DamageDealer { get; private set; } = new NullDamageDealer();
            public IElement Element { get; private set; } = new NullElement();
            public DamagePopupStyle DamagePopupStyle { get; private set; } = DamagePopupStyle.Default;
        }
    }
}