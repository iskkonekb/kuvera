using System;
using System.Collections.Generic;
using iskkonekb.kuvera.model;
using System.Linq;

namespace iskkonekb.kuvera.engine
{
    public class Accounts : IAccounts
    {
        public Accounts()
        {
        }

        private List<Entry> _entries = new List<Entry>();

        public void SetEntries(List<Entry> value) { _entries = value; }

        /// <summary>
                                       /// Получить исходящий остаток на конец дня по дате
                                       /// </summary>
                                       /// <param name="account">Счет для проверки</param>
                                       /// <param name="dt">Дата проверки остака</param>
                                       /// <returns>Исходящий остаток</returns>
        public decimal GetOutRest(Account account, DateTime dt)
        {
            // Окончание периода принимаем = + 1 день - одна миллисекунда
            DateTime dtEnd = dt.Date.AddDays(1) - new TimeSpan(0, 0, 0, 0, 1);
            return account.InitialSaldo + Sum(account.DateCreate, dtEnd, account );
        }

        #region #### Accont internal methods ####
        /// <summary>
        /// Расчет оборотов по счету
        /// </summary>
        /// <param name="startdate">Дата и время начала периода</param>
        /// <param name="enddate">Дата и время окончания периода</param>
        /// <param name="account">Счет</param>
        /// <returns></returns>
        internal decimal Sum(DateTime startdate, DateTime enddate, Account account)
        {
            if (_entries.Count == 0) return 0;
            EntryType[] incTypes = { EntryType.Income, EntryType.Transfer };
            EntryType[] outTypes = { EntryType.Outcome, EntryType.Transfer };
            // Сумма зачислений
            decimal incSum = _entries.Where(it => incTypes.Contains(it.Type) &&
            it.AcceptTime >= startdate && it.AcceptTime <= enddate
            && it.Income == account
            ).Sum(it => it.Value);
            //Сумма списаний
            decimal outSum = _entries.Where(it => outTypes.Contains(it.Type) &&
            it.AcceptTime >= startdate && it.AcceptTime <= enddate
            && it.Outcome == account
            ).Sum(it => it.Value);
            //Сумма оборотов по счету в диаазоне дат
            return incSum - outSum;
        }
        #endregion
    }
}