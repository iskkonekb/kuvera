using iskkonekb.kuvera.model;
using System;
using System.Collections.Generic;

namespace iskkonekb.kuvera.engine
{
    public interface IAccounts
    {
        /// <summary>
        /// Получить исходящий остаток на конец дня по дате
        /// </summary>
        /// <param name="account">Счет для проверки</param>
        /// <param name="dt">Дата проверки остака</param>
        /// <returns>Исходящий остаток</returns>
        decimal GetOutRest(model.Account account, DateTime dt);
    }
    public interface IEngine
    {
    }
    public interface IDepartments
    {
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
    public interface IEntries
    {
        /// <summary>
        /// Провести набор проводок
        /// </summary>
        /// <param name="entries">Проводки ддля проведения</param>
        void RegisterEntries(IEnumerable<Entry> entries);
        /// <summary>
        /// Сохранить в кэш набор проводок
        /// </summary>
        /// <param name="entries">Проводки ддля проведения</param>
        IEnumerable<Entry> GetEntries(Account account, DateTime startdate, DateTime enddate);
    }
}