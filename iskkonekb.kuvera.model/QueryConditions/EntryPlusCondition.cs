using System.Collections.Generic;
using System.Linq;
using iskkonekb.kuvera.core;

namespace iskkonekb.kuvera.model.QueryConditions
{
    public class EntryPlusCondition : ICondition
    {
        /// <summary>
        /// Тип операции. Приход/Расход
        /// </summary>
        public bool? Plus { get; set; }
        public virtual IEnumerable<T> Apply<T>(IEnumerable<T> query)
        {
            IEnumerable<IEntry> tt = (IEnumerable<IEntry>)query;
            if (Plus == null) return (IEnumerable<T>)tt;
            tt = tt.Where(x => x.Plus == Plus);
            return (IEnumerable<T>)tt;
        }
    }
}