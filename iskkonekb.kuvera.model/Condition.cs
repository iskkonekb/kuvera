using System;
using System.Collections.Generic;
using System.Linq;

namespace iskkonekb.kuvera.model
{
    public interface ICondition
    {
        IEnumerable<T> Apply<T>(IEnumerable<T> query);
    }
    public abstract class BaseEntryCondition
    {
        /// <summary>
        /// Заменить EntryType Both, Any на Income, Outcome
        /// </summary>
        /// <param name="arr">Массив типов проводок для замены</param>
        /// <param name="val">Заменяемый элемент массива</param>
        /// <returns></returns>
        protected EntryType[] ReplaceEntryType(EntryType[] arr, EntryType val)
        {
            EntryType[] ret = arr;
            if (ret.Contains(val))
            {
                ret = ret.Except(new EntryType[] { val }).ToArray();
                if (!ret.Contains(EntryType.Income))
                {
                    Array.Resize(ref ret, ret.Count() + 1);
                    ret[ret.Length - 1] = EntryType.Income;
                }
                if (!ret.Contains(EntryType.Outcome))
                {
                    Array.Resize(ref ret, ret.Count() + 1);
                    ret[ret.Length - 1] = EntryType.Outcome;
                }
                if (!ret.Contains(EntryType.Transfer))
                {
                    Array.Resize(ref ret, ret.Count() + 1);
                    ret[ret.Length - 1] = EntryType.Transfer;
                }
            }
            return ret;
        }
    }
    public struct DateTimeRange
    {
        public DateTime From;
        public DateTime To;
    }
    /// <summary>
    /// Отбор по дате проводки
    /// </summary>
    public class AcceptTimeCondition : ICondition
    {
        private DateTimeRange _AcceptTime;
        public AcceptTimeCondition()
        {
            _AcceptTime.From = EngineConsts.NullDate;
            _AcceptTime.To = EngineConsts.NullDate;
        }
        /// <summary>
        /// Период проводки
        /// </summary>
        public DateTimeRange AcceptTime { get { return _AcceptTime; } set { _AcceptTime = value; } }
        public IEnumerable<T> Apply<T>(IEnumerable<T> query)
        {
            IEnumerable<Entry> tt = (IEnumerable<Entry>)query;
            return (IEnumerable<T>)tt.Where(x => (x.AcceptTime >= _AcceptTime.From || _AcceptTime.From == EngineConsts.NullDate)
                    && x.AcceptTime <= _AcceptTime.To);
        }
    }
    /// <summary>
    /// Отбор по подразделению
    /// </summary>
    public class DepartmentCondition : BaseEntryCondition, ICondition
    {
        private EntryType[] _Types;
        private Department _Department;
        /// <summary>
        /// Тип операции. Приход/Расход
        /// </summary>
        public EntryType[] Types { get => _Types; set => _Types = value; }
        /// <summary>
        /// Подразделение
        /// </summary>
        public Department Department { get => _Department; set => _Department = value; }
        //Подготовка массива типов проводок
        private EntryType[] PrepareEntryType(EntryType[] arr)
        {
            EntryType[] rowTypes = _Types;
            rowTypes = ReplaceEntryType(rowTypes, EntryType.Any);   //Меняем Any на Income, Outcome
            return rowTypes;
        }
        public IEnumerable<T> Apply<T>(IEnumerable<T> query)
        {
            IEnumerable<Entry> tt = (IEnumerable<Entry>)query; //Промежуточный результат
            char[] queryType = new char[2] { '0', '0' };
            if (_Types.Contains(EntryType.Income))
                queryType[0] = '1';
            if (_Types.Contains(EntryType.Outcome))
                queryType[1] = '1';
            string s = new string(queryType);
            EntryType[] rowTypes = PrepareEntryType(_Types); //Подготавливаем массив
            switch (s)
            {
                case "00":
                case "11":
                    tt = tt.Where(x => ((x.Outcome != null && x.Outcome.Department == _Department) || (x.Income != null && x.Income.Department == _Department)) && rowTypes.Contains(x.Type));
                    break;
                case "10":
                    tt = tt.Where(x => (x.Income != null && x.Income.Department == _Department) && rowTypes.Contains(x.Type));
                    break;
                case "01":
                    tt = tt.Where(x => (x.Outcome != null && x.Outcome.Department == _Department) && rowTypes.Contains(x.Type));
                    break;
            }
            return (IEnumerable<T>)tt;
        }
     }
    /// <summary>
    /// Отбор по типу проводки. Например Доход/Расхолд
    /// </summary>
    public class EntryTypeCondition : BaseEntryCondition, ICondition
    {
        private EntryType[] _Types;
        public EntryType[] Types { get { return _Types; } set { _Types = value; } }

        public EntryTypeCondition()
        {
            _Types = new[] { EntryType.Any };
        }
        public IEnumerable<T> Apply<T>(IEnumerable<T> query)
        {
            IEnumerable<Entry> tt = (IEnumerable<Entry>)query;
            if (_Types.Contains(EntryType.Any)) return query;
            EntryType[] rowTypes = _Types;
            if (rowTypes.Count() == 1)
                tt = tt.Where(x => x.Type == rowTypes[0]);
            else
                tt = tt.Where(x => rowTypes.Contains(x.Type)); //stackoverflow утверждает что Linq для EF трансформирует эту конструкцию в IN... Проверим
            return (IEnumerable<T>)tt;
        }
    }
    /// <summary>
    /// Отбор по счету
    /// </summary>
    public class AccountCondition : BaseEntryCondition, ICondition
    {
        private EntryType[] _Types;
        private Account _Account;
        /// <summary>
        /// Тип операции. Приход/Расход/Перевод
        /// </summary>
        public EntryType[] Types { get => _Types; set => _Types = value; }
        /// <summary>
        /// Подразделение
        /// </summary>
        public Account Account { get => _Account; set => _Account = value; }
        //Подготовка массива типов проводок
        private EntryType[] PrepareEntryType(EntryType[] arr)
        {
            EntryType[] rowTypes = _Types;
            rowTypes = ReplaceEntryType(rowTypes, EntryType.Any);   //Меняем Any на Income, Outcome, Transfer
            return rowTypes;
        }
        public IEnumerable<T> Apply<T>(IEnumerable<T> query)
        {
            IEnumerable<Entry> tt = (IEnumerable<Entry>)query; //Промежуточный результат
            char[] queryType = new char[2] { '0', '0'};
            if (_Types.Contains(EntryType.Income))
                queryType[0] = '1';
            if (_Types.Contains(EntryType.Outcome))
                queryType[1] = '1';
            EntryType[] rowTypes = PrepareEntryType(_Types); //Подготавливаем массив
            string s = new string(queryType);
            switch (s)
            {
                case "00":
                case "11":
                    tt = tt.Where(x => ((x.Outcome != null && x.Outcome == _Account) || (x.Income != null && x.Income == _Account)) && rowTypes.Contains(x.Type));
                    break;
                case "10":
                    tt = tt.Where(x => (x.Income != null && x.Income == _Account) && rowTypes.Contains(x.Type));
                    break;
                case "01":
                    tt = tt.Where(x => (x.Outcome != null && x.Outcome == _Account) && rowTypes.Contains(x.Type));
                    break;
            }
            return (IEnumerable<T>) tt;
        }
    }
}