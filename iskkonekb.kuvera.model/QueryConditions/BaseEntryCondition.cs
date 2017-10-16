using System.Collections.Generic;
using System.Linq;
using iskkonekb.kuvera.core;

namespace iskkonekb.kuvera.model.QueryConditions
{
    /// <summary>
    /// Базовый класс условий фильтрации проводок
    /// </summary>
    public class BaseEntryCondition
    {
        public bool? IncludeTransfer { get; set; }
        /// <summary>
        /// Тип операции. Приход/Расход
        /// </summary>
        public bool? Plus { get; set; }
        public BaseEntryCondition()
        {
        }
         public virtual IEnumerable<T> Apply<T>(IEnumerable<T> query)
        {
            IEnumerable<IEntry> tt = (IEnumerable<IEntry>)query;
            if (Plus != null)
                tt = tt.Where(x => x.Plus == Plus);
            if (IncludeTransfer == false) //Исключать переводы
                tt = tt.Where(x => x.Transfer == false);
             return (IEnumerable<T>)tt;
        }
    }
}