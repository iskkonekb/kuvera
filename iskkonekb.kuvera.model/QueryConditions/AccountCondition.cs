using System.Collections.Generic;
using System.Linq;
using iskkonekb.kuvera.core;
using System;
using System.Linq.Expressions;

namespace iskkonekb.kuvera.model.QueryConditions
{
    /// <summary>
    /// Отбор по счету
    /// </summary>
    public class AccountCondition : BaseEntryCondition, ICondition
    {
        /// <summary>
        /// Подразделение
        /// </summary>
        public Account Account { get; set; }

        public override IEnumerable<T> Apply<T>(IEnumerable<T> query)
        {
            IEnumerable<IEntry> tt = (IEnumerable<IEntry>)query;
            tt = tt.Where(x => x.Account == Account);
            tt = base.Apply(tt);
            return (IEnumerable<T>)tt;
        }
    }
}