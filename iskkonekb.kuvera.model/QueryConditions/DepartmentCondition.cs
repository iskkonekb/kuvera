using System.Collections.Generic;
using System.Linq;
using iskkonekb.kuvera.core;

namespace iskkonekb.kuvera.model.QueryConditions
{
    /// <summary>
    /// Отбор по подразделению
    /// </summary>
    public class DepartmentCondition : BaseEntryCondition, ICondition
    {
        /// <summary>
        /// Подразделение
        /// </summary>
        public Department Department { get; set; }
        public override IEnumerable<T> Apply<T>(IEnumerable<T> query)
        {
            IEnumerable<IEntry> tt = (IEnumerable<IEntry>)query; //Промежуточный результат
            tt = tt.Where(x => x.Account.Department == Department);
            if (Plus == null && IncludeTransfer == true) //Проводки по любой стороне, включая переводы
            {
                var _plus = (Plus == null) ? true : Plus;
                //Исключаем вторую часть проводки, чтобы избежать задвоения оборотов по переводам
                tt = tt.Where(x => x.Transfer == false || (x.Transfer == true && x.Plus == _plus));
            }
            tt = base.Apply(tt);
            return (IEnumerable<T>)tt;
        }
    }
}