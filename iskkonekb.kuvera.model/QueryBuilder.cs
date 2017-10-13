using System;
using System.Collections.Generic;
using iskkonekb.kuvera.model;
using iskkonekb.kuvera.model.QueryConditions;

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
            EQuery qParent = new EQuery {
                AcceptTime = new DateTimeRange { From = EngineConsts.NullDate, To = dt}
            };
            EQuery q = new EQuery {
                QueryType = QueryTypes.Sum,
                SubQueries = {
                    new EQuery {
                        Parent = qParent,
                        EntryType = new EntryType[]{ EntryType.Income, EntryType.Transfer},
                        Account = new AccountCondition{ Account = account},
                    },
                    new EQuery {
                        Parent = qParent,
                        EntryType = new EntryType[]{ EntryType.Outcome, EntryType.Transfer},
                        Account = new AccountCondition{ Account = account},
                        Negate = true
                    }
                }
            };
            return q;
        }

        /// <summary>
        /// Расчет исх. остатка по департаменту
        /// </summary>
        /// <param name="department">Подразделение</param>
        /// <param name="dt">Дата и время окончания периода</param>
        /// <returns></returns>
        public static Query DepartSaldoOut(Department department, DateTime dt)
        {
            EQuery qParent = new EQuery
            {
                AcceptTime = new DateTimeRange { From = EngineConsts.NullDate, To = dt }
            };
            EQuery q = new EQuery
            {
                QueryType = QueryTypes.Sum,
                SubQueries = {
                    new EQuery {
                        Parent = qParent,
                        EntryType = new EntryType[]{ EntryType.Income, EntryType.Transfer},
                        Department = new DepartmentCondition{  Department = department},
                    },
                    new EQuery {
                        Parent = qParent,
                        EntryType = new EntryType[]{ EntryType.Outcome, EntryType.Transfer},
                        Department = new DepartmentCondition{  Department = department},
                        Negate = true
                    }
                }
            };
            return q;
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
            EQuery qParent = new EQuery
            {
                AcceptTime = new DateTimeRange { From = startdate, To = enddate }
            };
            EQuery q = new EQuery
            {
                QueryType = QueryTypes.Sum,
                SubQueries = {
                    new EQuery {
                        Parent = qParent,
                        EntryType = new EntryType[]{ type},
                        Department = new DepartmentCondition{  Department = department},
                    },
                }
            };
            return q;
        }
    }
}