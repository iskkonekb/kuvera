using iskkonekb.kuvera.model;
using System;
using System.Collections.Generic;

namespace iskkonekb.kuvera.engine.test
{
    public interface IEngine
    {
        /// <summary>
        /// Провести набор проводок
        /// </summary>
        /// <param name="entries">Проводки ддля проведения</param>
        void Accept(IEnumerable<Entry> entries);

        /// <summary>
        /// Получить исходящий остаток на конец дня по дате
        /// </summary>
        /// <param name="account">Счет для проверки</param>
        /// <param name="dt">Дата проверки остака</param>
        /// <returns>Исходящий остаток</returns>
        decimal GetOutRest(Account account, DateTime dt);
    }
}