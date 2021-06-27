using System.Collections.Generic;

namespace Elysium.Combat
{
    public interface IModelBuilder
    {
        T GetModels<T>();
    }
}