using iskkonekb.kuvera.model;
using System;
using System.Collections.Generic;

namespace iskkonekb.kuvera.engine
{
    public interface IEngine
    {
        /// <summary>
        /// Провести набор проводок
        /// </summary>
        /// <param name="entries">Проводки ддля проведения</param>
        void RegisterEntries(IEnumerable<Entry> entries);

        /// <summary>
        /// Расчет оборотов по счету
        /// </summary>
        /// <param name="startdate">Дата начала периода</param>
        /// <param name="enddate">Дата окончания периода</param>
        /// <param name="account">Счет</param>
        /// <param name="type">Спписание/Зачисление</param>
        /// <returns></returns>
        decimal Sum(DateTime startdate, DateTime enddate, Account account, EntryType type);

        /// <summary>
        /// Расчет исх. остатка по счету
        /// </summary>
        /// <param name="account">Счет</param>
        /// <param name="dt">Дата и время окончания периода</param>
        /// <returns></returns>
        decimal SaldoOut(Account account, DateTime dt);

        /// <summary>
        /// Получить начальный остаток по подразделению
        /// </summary>
        /// <param name="depart">Подразделение</param>
        /// <returns></returns>
        decimal InitSaldo(Department department);
 
        /// <summary>
        /// Сумма оборотов по департаменту за период
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="department"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        decimal Sum(DateTime startdate, DateTime enddate, Department department, EntryType type);
    }
}