using System;
using System.Collections.Generic;
using iskkonekb.kuvera.model;

namespace iskkonekb.kuvera.model
{
    public class QueryBuilder
    {
        /// <summary>
        /// Расчет исх. остатка по департаменту
        /// </summary>
        /// <param name="account">Счет</param>
        /// <param name="dt">Дата и время окончания периода</param>
        /// <returns></returns>
        public static Query AccountSaldoOut(Account account, DateTime dt)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Расчет исх. остатка по департаменту
        /// </summary>
        /// <param name="department">Подразделение</param>
        /// <param name="dt">Дата и время окончания периода</param>
        /// <returns></returns>
        public static Query DepartSaldoOut(Department department, DateTime dt)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сумма доходов/расходов по департаменту за период
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="department"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Query SumDepart(DateTime startdate, DateTime enddate, Department department, EntryType type)
        {
            throw new NotImplementedException();
        }
    }
}