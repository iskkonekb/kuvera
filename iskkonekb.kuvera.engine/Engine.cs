using System;
using System.Collections.Generic;
using iskkonekb.kuvera.model;
using System.Linq;

namespace iskkonekb.kuvera.engine
{
    public static class EngineConsts
    {
        public static DateTime NullDate { get { return new DateTime(1900, 1, 1); } }
    }
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
        /// Получить начальный остаток по департаменту
        /// </summary>
        /// <param name="depart">Подразделение</param>
        /// <returns></returns>
        public decimal InitSaldo(Department department)
        {
            if (_entries.Count == 0) return 0;
            return _entries.Where(x => x.Type == EntryType.Income &&
            x.Income.Department == department
            && ((x.Category != null) ? x.Category.Code : String.Empty) == SysCategory.initSaldo.ToString()
            ).Sum(it => it.Value);
        }
        /// <summary>
        /// Получить начальный остаток по счету
        /// </summary>
        /// <param name="account">Счет</param>
        /// <returns></returns>
        public decimal InitSaldo(Account account)
        {
            if (_entries.Count == 0) return 0;
            return _entries.Where(x => x.Type == EntryType.Income &&
            x.Income == account
            && ((x.Category != null) ? x.Category.Code : String.Empty) == SysCategory.initSaldo.ToString()
            ).Sum(it => it.Value);
        }

        /// <summary>
        /// Сумма доходов/расходов по департаменту за период
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="department"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public decimal Sum(DateTime startdate, DateTime enddate, Department department, EntryType type)
        {
            if (_entries.Count == 0) return 0;
            return _entries.Where(x => x.Type == type &&
            (x.AcceptTime >= startdate || startdate == EngineConsts.NullDate) && x.AcceptTime <= enddate
            && (type == EntryType.Income ? x.Income : x.Outcome).Department == department
            && ((x.Category != null) ? x.Category.Code : String.Empty) != SysCategory.initSaldo.ToString()
            ).Sum(it => it.Value);
        }
        /// <summary>
        /// Сумма доходов/расходов по счету за период
        /// </summary>
        /// <param name="startdate">Дата начала периода</param>
        /// <param name="enddate">Дата окончания периода</param>
        /// <param name="account">Счет</param>
        /// <param name="type">Спписание/Зачисление</param>
        /// <returns></returns>
        public decimal Sum(DateTime startdate, DateTime enddate, Account account, EntryType type)
        {
            if (_entries.Count == 0) return 0;
            List<EntryType> types = new List<EntryType>
            {
                type,
                EntryType.Transfer
            };
            return _entries.Where(x => types.Contains(x.Type) &&
            x.AcceptTime >= startdate && x.AcceptTime <= enddate &&
            (type == EntryType.Income ? x.Income : x.Outcome) == account
            && ((x.Category != null) ? x.Category.Code : String.Empty) != SysCategory.initSaldo.ToString()
            ).Sum(it => it.Value);
        }

        public decimal Sum(Query query)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Расчет исх. остатка по департаменту
        /// </summary>
        /// <param name="account">Счет</param>
        /// <param name="dt">Дата и время окончания периода</param>
        /// <returns></returns>
        public decimal SaldoOut(DateTime dt, Department department)
        {
            if (_entries.Count == 0) return 0;
            decimal initSaldo = InitSaldo(department);
            decimal incSum = Sum(EngineConsts.NullDate, dt, department, EntryType.Income);    // Сумма зачислений
            decimal outSum = Sum(EngineConsts.NullDate, dt, department, EntryType.Outcome);    //Сумма списаний
            //Сумма оборотов по счету в диаазоне дат
            return initSaldo + incSum - outSum;
        }
        /// <summary>
        /// Расчет исх. остатка по счету
        /// </summary>
        /// <param name="account">Счет</param>
        /// <param name="dt">Дата и время окончания периода</param>
        /// <returns></returns>
        public decimal SaldoOut(Account account, DateTime dt)
        {
            if (_entries.Count == 0) return 0;
            decimal initSaldo = InitSaldo(account);
            decimal incSum = Sum(account.DateCreate, dt, account, EntryType.Income);    // Сумма зачислений
            decimal outSum = Sum(account.DateCreate, dt, account, EntryType.Outcome);    //Сумма списаний
            //Сумма оборотов по счету в диаазоне дат
            return initSaldo + incSum - outSum;
        }

        public Engine()
        {
        }
    }
}