using System;
using System.Collections.Generic;
using iskkonekb.kuvera.model;
using System.Linq;

namespace iskkonekb.kuvera.engine
{
    public class Engine : IEngine
    {
        private List<Entry> _entries = new List<Entry>();

        /// <summary>
        /// Провести набор проводок
        /// </summary>
        /// <param name="entries">Проводки ддля проведения</param>
        public void RegisterEntries(IEnumerable<Entry> entries)
        {
            _entries.AddRange(entries);
        }
        /// <summary>
        /// Рассчитать сумму проводок
        /// </summary>
        /// <param name="query">Запрос для расчета</param>
        /// <returns></returns>
        public decimal Sum(IQuery query)
        {
            decimal result = 0m;
            decimal rawresult = 0m;
            if (query.QueryType == QueryTypes.Primary)
                rawresult = PrimarySum(query);
            else if (query.QueryType == QueryTypes.Sum)
                rawresult = query.CollectSubresults(x => Sum(x)).Sum(); // рекурсивный вызов с суммированием
            else
                throw new NotImplementedException("Пока формулы не поддерживаются");
            result = AdaptResult(rawresult, query);
            return result;
        }

        /// <summary>
        /// Пост обработка результатов вычислений
        /// </summary>
        /// <param name="result">Рассчитанный результат</param>
        /// <param name="query">Запрос для которого рассчитываются проводки</param>
        /// <returns></returns>
        decimal AdaptResult(decimal result, IQuery query)
        {
            if (query.IgnoreMinus && result < 0) return 0;
            if (query.Negate) return -result;
            return result;
        }
        /// <summary>
        /// Расчет суммы по основномуу запросу
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <returns>Рассчитанная сумма по проводкам</returns>
        decimal PrimarySum(IQuery query)
        {
            return query.Apply(_entries).Sum(x => x.Value);
        }

        public Engine()
        {
        }
    }
}