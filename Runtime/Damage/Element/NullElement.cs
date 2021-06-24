namespace Elysium.Combat
{
    public class NullElement : IElement
    {
        public DamagePopupStyle GetStyle(bool crit)
        {
            return DamagePopupStyle.Default;
        }
    }
}