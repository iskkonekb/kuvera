using System;
using System.Collections.Generic;
using System.Linq;

namespace iskkonekb.kuvera.model
{
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
        public DateTimeRange AcceptTime { get => _AcceptTime; set => _AcceptTime = value; }
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
    public class DepartmentCondition : ICondition
    {
        private EntryType _Type;
        private Department _Department;
        /// <summary>
        /// Тип операции. Приход/Расход
        /// </summary>
        public EntryType Type { get => _Type; set => _Type = value; }
        /// <summary>
        /// Подразделение
        /// </summary>
        public Department Department { get => _Department; set => _Department = value; }
        public DepartmentCondition()
        {
        }

        public IEnumerable<T> Apply<T>(IEnumerable<T> query)
        {
            // !!! Здесь хорошо бы сделать лямбда конструктор чтобы условие x.Type == _Type дважды не попадало вв LINQ
            IEnumerable<Entry> tt = (IEnumerable<Entry>)query;
            return (IEnumerable<T>)tt.Where(x => x.Type == _Type &&
            (_Type == EntryType.Income ? x.Income : x.Outcome).Department == Department);
        }
    }

    /// <summary>
    /// Реализация Query для Entries
    /// </summary>
    public class EQuery : Query
    {
        private AcceptTimeCondition _AcceptTime;
        private DepartmentCondition _Department;
        private void _addCondition(ICondition condition)
        {
            where.Remove(condition);
            if (condition != null)
            {
                where.Add(condition);
            }
        }

        public EQuery()
        {
            _AcceptTime = new AcceptTimeCondition();
            _Department = new DepartmentCondition();
        }
        /// <summary>
        /// условие для периода проводки
        /// </summary>
        public DateTimeRange AcceptTime;
        /// <summary>
        /// Подразделение
        /// </summary>
        public Department Department;
        /// <summary>
        /// Тип операции. Приход/Расход
        /// </summary>
        public EntryType Type;
        //Добавить условие по периоду
        private void _addAcceptTimeCond()
        {
            //Условие по периоду
            if (AcceptTime.From != EngineConsts.NullDate || AcceptTime.To != EngineConsts.NullDate)
            {
                _AcceptTime.AcceptTime = AcceptTime;
                _addCondition(_AcceptTime);
            }
            else if (where.Contains(_AcceptTime))
                where.Remove(_AcceptTime);
        }
        //Добавить условие по подразделениям
        private void _addDepartCond()
        {
            if (Department != null)
            {
                _Department.Department = Department;
                _Department.Type = Type;
                _addCondition(_Department);
            }
            else if (where.Contains(_Department))
                where.Remove(_Department);
        }
        public override IEnumerable<T> Filter<T>(IEnumerable<T> srcArr)
        {
            _addAcceptTimeCond(); //Условие по периоду
            _addDepartCond(); //Условие по подразделениям
            return base.Filter(srcArr);
        }
    }
}