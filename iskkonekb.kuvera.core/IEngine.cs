using iskkonekb.kuvera.model;
using System;
using System.Collections.Generic;

namespace iskkonekb.kuvera.core
{
    public interface IEngine
    {
        /// <summary>
        /// Провести набор проводок
        /// </summary>
        /// <param name="entries">Проводки ддля проведения</param>
        void RegisterEntries(IEnumerable<IEntry> entries);

        /// <summary>
        /// Рассчитать сумму проводок
        /// </summary>
        /// <param name="query">Запрос для расчета</param>
        /// <returns></returns>
        decimal Sum(IQuery query);
    }
}