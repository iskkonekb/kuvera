using System.Collections.Generic;
using System.Linq;
using iskkonekb.kuvera.core;

namespace iskkonekb.kuvera.model.QueryConditions
{
   /// <summary>
   /// Условия фильтрации переводов. Вклчать не включать переводы
   /// </summary>
    public class IncludeTransferCondition : ICondition
    {
        /// <summary>
        /// Включать или не включать переводы. true - включать
        /// </summary>
        public bool IncludeTransfer { get; set; }
        public virtual IEnumerable<T> Apply<T>(IEnumerable<T> query)
        {
            IEnumerable<IEntry> tt = (IEnumerable<IEntry>)query;
            if (IncludeTransfer == false)
                tt = tt.Where(x => x.Transfer == IncludeTransfer);
            return (IEnumerable<T>)tt;
        }
    }
}