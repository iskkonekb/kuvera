using System;
using System.Collections.Generic;
using System.Linq;

namespace iskkonekb.kuvera.model
{
    /// <summary>
    /// Реализация Query для Entries
    /// </summary>
    public class EQuery : Query
    {
        private AcceptTimeCondition _AcceptTimeCondition;
        private DepartmentCondition _DepartmentCondition;
        private EntryTypeCondition _EntryTypeCondition;
        private AccountCondition _AccountCondition;
        /// <summary>
        /// Добавить уусловие в массив
        /// </summary>
        /// <param name="condition"></param>
        private void _addCondition(ICondition condition)
        {
            if (condition == null) return;
            if (Conditions.Contains(condition))
                Conditions.Remove(condition);
            Conditions.Add(condition);
        }
        /// <summary>
        /// Умолчательный конструктор
        /// </summary>
        public EQuery()
        {
        }
        /// <summary>
        /// Запомнить условие по сроку
        /// </summary>
        /// <param name="value">Период</param>
        private void SetAcceptTimeCondition(DateTimeRange value)
        {
            if (Conditions.Contains(_AcceptTimeCondition)) Conditions.Remove(_AcceptTimeCondition);
            _AcceptTimeCondition = new AcceptTimeCondition { AcceptTime = value };
            if (_AcceptTimeCondition != null) Conditions.Add(_AcceptTimeCondition);
        }
        /// <summary>
        /// условие для периода проводки
        /// </summary>
        public DateTimeRange AcceptTime { get => _AcceptTimeCondition.AcceptTime; set { SetAcceptTimeCondition( value); } }
        private void SetDepartCondition(DepartmentCondition value)
        {
            if (Conditions.Contains(_DepartmentCondition)) Conditions.Remove(_DepartmentCondition);
            _DepartmentCondition = value;
            if (_DepartmentCondition != null) Conditions.Add(_DepartmentCondition);
        }
        /// <summary>
        /// Подразделение
        /// </summary>
        public DepartmentCondition Department { get => _DepartmentCondition; set => SetDepartCondition(value); }
        /// <summary>
        /// Установка условия отбора по счету
        /// </summary>
        /// <param name="value"></param>
        private void SetAccountCondition(AccountCondition value)
        {
            if (Conditions.Contains(_AccountCondition)) Conditions.Remove(_AccountCondition);
            _AccountCondition = value;
            if (_AccountCondition != null) Conditions.Add(_AccountCondition);
        }
        /// <summary>
        /// Условие по счету
        /// </summary>
        public AccountCondition Account { get => _AccountCondition; set => SetAccountCondition(value); }
        private void SetEntryTypeCondition(EntryType[] value)
        {
            if (Conditions.Contains(_EntryTypeCondition)) Conditions.Remove(_EntryTypeCondition);
            _EntryTypeCondition = new EntryTypeCondition { Types = value };
            if (_EntryTypeCondition != null) Conditions.Add(_EntryTypeCondition);
        }
        /// <summary>
        /// Тип операции. Приход/Расход
        /// </summary>
        public EntryType[] EntryType { get => _EntryTypeCondition.Types; set => SetEntryTypeCondition(value); }
        /// <summary>
        /// Добавить условие по периоду
        /// </summary>
        private void _addAcceptTimeCondition()
        {
            //Условие по периоду
            if (_AcceptTimeCondition == null) return;
            if (_AcceptTimeCondition.AcceptTime.From != EngineConsts.NullDate || _AcceptTimeCondition.AcceptTime.To != EngineConsts.NullDate)
            {
                _addCondition(_AcceptTimeCondition);
            }
            else if (Conditions.Contains(_AcceptTimeCondition))
                Conditions.Remove(_AcceptTimeCondition);
        }
        public override IEnumerable<T> Apply<T>(IEnumerable<T> srcArr)
        {
            _addAcceptTimeCondition(); //Условие по периоду
            _addCondition(_EntryTypeCondition); //Добавить условие по стороне проводки
            if (_DepartmentCondition != null)
                if (_DepartmentCondition.Types == null)
                    _DepartmentCondition.Types = _EntryTypeCondition.Types;
            if (_AccountCondition != null)
                if (_AccountCondition.Types == null)
                    _AccountCondition.Types = _EntryTypeCondition.Types;
            return base.Apply(srcArr);
        }
    }
}