using System;
using System.Collections.Generic;
using iskkonekb.kuvera.model.QueryConditions;
using iskkonekb.kuvera.core;

namespace iskkonekb.kuvera.model
{
    public class Query : IQuery
    {
        public IQuery Parent { get; set; }
        public List<ICondition> Conditions { get; private set; } = new List<ICondition>();
        /// <summary>
        /// Режим влияния подзапроса на результирующую выборку.
        /// </summary>
        public QueryTypes QueryType { get; set; }
        public bool IgnoreMinus { get; set; }
        public List<IQuery> SubQueries { get; private set; } = new List<IQuery>();
        public bool Negate { get; set; }
        public string Comment { get; set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        public Query()
        {
            //Default sets
            Negate = false;
            IgnoreMinus = false;
            QueryType = QueryTypes.Primary;
        }
        /// <summary>
        /// Применить условия текущего запроса
        /// </summary>
        /// <typeparam name="T">Коллекция произвольного типа</typeparam>
        /// <param name="srcArr">Массив Исодных данных, для которыхх ппприменяются условия</param>
        /// <returns></returns>
        public virtual IEnumerable<T> Apply<T>(IEnumerable<T> srcArr)
        {
            var result = srcArr;
            //Фильтр родительского Query
            if (Parent != null)
                result = Parent.Apply(result);
            //Филтры по условиям текущего Query
            foreach (var x in Conditions)
                result = x.Apply(result);
            return result;
        }
        /// <summary>
        /// Вернуть коллекцию запросов передданного запроса-контейнера
        /// </summary>
        /// <typeparam name="T">Тип результата</typeparam>
        /// <param name="collector">Фнкция обработки коллекции запросов</param>
        /// <returns></returns>
        public IEnumerable<T> CollectSubresults<T>(Func<IQuery, T> collector)
        {
            if (SubQueries != null)
                foreach (var x in SubQueries)
                    yield return collector(x);
        }
    }
}