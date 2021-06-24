namespace Elysium.Combat
{
    public interface ISource
    {
        IDamageDealer DamageDealer { get; }
        IElement Element { get; }
        DamagePopupStyle DamagePopupStyle { get; }
    }
}