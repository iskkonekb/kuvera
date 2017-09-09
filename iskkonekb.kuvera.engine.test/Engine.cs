using System;
using System.Collections.Generic;
using iskkonekb.kuvera.model;
using System.Linq;

namespace iskkonekb.kuvera.engine.test
{
    public class Engine : IEngine
    {
        public Engine()
        {
        }

        private List<Entry> _entries = new List<Entry>();

        /// <summary>
        /// Провести набор проводок
        /// </summary>
        /// <param name="entries">Проводки ддля проведения</param>
        public void Accept(IEnumerable<Entry> entries)
        {
            _entries.AddRange(entries);
        }

        /// <summary>
        /// Получить исходящий остаток на конец дня по дате
        /// </summary>
        /// <param name="account">Счет для проверки</param>
        /// <param name="dt">Дата проверки остака</param>
        /// <returns>Исходящий остаток</returns>
        public decimal GetOutRest(Account account, DateTime dt)
        {
            throw new NotImplementedException();
        }

        internal decimal Sum(DateTime startdate, DateTime enddate, Department department, EntryType type)
        {
            return _entries.Where(it => it.Type == type &&
            it.AcceptTime>=startdate && it.AcceptTime<=enddate
            && (type==EntryType.Income?it.Income:it.Outcome).Department==department
            ).Sum(it => it.Value);
        }

        internal int GetInRest(Department kitchen, DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        internal int GeOutRest(Department kitchen, DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}