using System.Collections.Generic;
using System.Linq;

namespace iskkonekb.kuvera.model
{
    public enum QueryTypes
    {
        Select,
        Sum,
        Formula
    }
    /// <summary>
    /// Способ обработки результата у подзапросов для основного запроса
    /// </summary>
    public enum ResultSets
    {
        /// <summary>
        /// Результаты подзапросов будут объединены в итоговый набор
        /// </summary>
        Union,
        /// <summary>
        /// Подзапросы будут накладывать один за другим свои условия на поступивший набор
        /// </summary>
        Combined
    }
    public class Query
    {
        public bool Negate;
        public Query parent;
        public List<ICondition> where;
        /// <summary>
        /// Режим влияния подзапроса на результирующую выборку.
        /// </summary>
        public ResultSets Set;
        //public QueryTypes type;
        public List<Query> subQ;
        /// <summary>
        /// Конструктор
        /// </summary>
        public Query()
        {
            //Default sets
            Set = ResultSets.Union;
            where = new List<ICondition>();
            subQ = new List<Query>();
            Negate = false;
        }
        /// <summary>
        /// Применить условия текущего запроса
        /// </summary>
        /// <typeparam name="T">Коллекция произвольного типа</typeparam>
        /// <param name="srcArr">Массив Исодных данных, для которыхх ппприменяются условия</param>
        /// <returns></returns>
        public virtual IEnumerable<T> Filter<T>(IEnumerable<T> srcArr)
        {
            var result = srcArr;
            //Филлььтр родительского Query
            if (parent != null)
                result = parent.Filter(result);
            foreach (var x in where)
                result = x.Apply(result);
            //Применить условия подзапросов
            return FilterSubQueries(result);
        }
        /// <summary>
        /// Применить условия подзапросов для текущего запроса
        /// </summary>
        /// <typeparam name="T">Коллекция произвольного типа</typeparam>
        /// <param name="srcArr">Массив Исодных данных, для которыхх ппприменяются условия</param>
        /// <returns></returns>
        private IEnumerable<T> FilterSubQueries<T>(IEnumerable<T> srcArr)
        {
            IEnumerable<T> result = Enumerable.Empty<T>(); ;
            if (subQ == null) return srcArr;
            if (subQ.Count > 0)
                foreach (var x in subQ)
                {
                    //Вариант влияния дочерней выборки на родителя
                    if (Set == ResultSets.Union) //Объединяем подмножества
                        result = Enumerable.Concat(result, x.Filter(srcArr));
                    else //Ораничиваем результирующую выборку
                        result = x.Filter(srcArr);
                }
            else
                result = srcArr;
            return result;
        }
    }
}