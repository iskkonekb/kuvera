using System;
using System.Collections.Generic;

namespace iskkonekb.kuvera.core
{
    public interface IQuery
    {
        List<ICondition> Conditions { get; }
        bool IgnoreMinus { get; set; }
        bool Negate { get; set; }
        IQuery Parent { get; set; }
        QueryTypes QueryType { get; set; }
        List<IQuery> SubQueries { get; }

        IEnumerable<T> Apply<T>(IEnumerable<T> srcArr);
        IEnumerable<T> CollectSubresults<T>(Func<IQuery, T> collector);
    }
}