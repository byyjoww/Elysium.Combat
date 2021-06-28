namespace Elysium.Combat
{
    public interface IElementFactory
    {
        IElement GetElementByKeyOrDefault(Element _key);
        IElement GetElementByKeyOrDefault(string _key);
    }
}