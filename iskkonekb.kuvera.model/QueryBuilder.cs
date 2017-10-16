using System;
using System.Collections.Generic;
using iskkonekb.kuvera.core;
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
            EQuery qParent = new EQuery
            {
                Comment = "Parent Query",
                AcceptTime = new DateTimeRange { From = EngineConsts.NullDate, To = dt }
            };
            EQuery q = new EQuery
            {
                QueryType = QueryTypes.Sum,
                Comment = "Sum query",
                SubQueries = {
                    new EQuery {
                        Comment = "Account plus query",
                        Parent = qParent,
                        Account = new AccountCondition{ Account = account, IncludeTransfer = true, Plus = true},
                    },
                    new EQuery {
                        Comment = "Account minus query",
                        Parent = qParent,
                        Account = new AccountCondition{ Account = account, Plus = false, IncludeTransfer = true},
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
                        Department = new DepartmentCondition{  Department = department,  Plus = true, IncludeTransfer = true},
                    },
                    new EQuery {
                        Parent = qParent,
                        Department = new DepartmentCondition{  Department = department, Plus = false, IncludeTransfer = true},
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
        public static Query SumDepart(DateTime startdate, DateTime enddate, Department department, bool plus)
        {
            EQuery qParent = new EQuery
            {
                Comment = "Parent query",
                AcceptTime = new DateTimeRange { From = startdate, To = enddate }
            };
            EQuery q = new EQuery
            {
                Comment = "Sum query",
                QueryType = QueryTypes.Sum,
                SubQueries = {
                    new EQuery {
                        Comment = "Doxod query",
                        Parent = qParent,
                        Department = new DepartmentCondition{  Plus = plus, IncludeTransfer = false, Department = department},
                    }
                }
            };
            return q;
        }
    }
}