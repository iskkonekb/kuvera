using System.Collections.Generic;

namespace iskkonekb.kuvera.model
{
    public interface ICondition
    {
        IEnumerable<T> Apply<T>(IEnumerable<T> query);
    }

}