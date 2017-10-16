using System.Collections.Generic;

namespace iskkonekb.kuvera.core
{
    public interface ICondition
    {
        IEnumerable<T> Apply<T>(IEnumerable<T> query);
    }
}