using System.Collections.Generic;
using iskkonekb.kuvera.model.QueryConditions;
using iskkonekb.kuvera.core;

namespace iskkonekb.kuvera.model
{
    /// <summary>
    /// Реализация Query для Entries
    /// </summary>
    public class EQuery : Query
    {
        private AcceptTimeCondition _AcceptTimeCondition;
        private DepartmentCondition _DepartmentCondition;
        private AccountCondition _AccountCondition;
        private EntryPlusCondition _EntryPlusCondition;
        private IncludeTransferCondition _IncludeTransferCondition;
        /// <summary>
        /// Добавить уусловие в массив
        /// </summary>
        /// <param name="condition"></param>
        private void _addCondition(ICondition condition)
        {
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
            Conditions.Add(_AcceptTimeCondition);
        }
        /// <summary>
        /// условие для периода проводки
        /// </summary>
        public DateTimeRange AcceptTime { get => _AcceptTimeCondition.AcceptTime; set { SetAcceptTimeCondition(value); } }
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

        private void SetEntryPlusCondition(EntryPlusCondition value)
        {
            if (Conditions.Contains(_EntryPlusCondition)) Conditions.Remove(_EntryPlusCondition);
            _EntryPlusCondition = value;
            if (_EntryPlusCondition != null) Conditions.Add(_EntryPlusCondition);
        }
        /// <summary>
        /// Условие по счету
        /// </summary>
        public EntryPlusCondition Plus { get => _EntryPlusCondition; set => SetEntryPlusCondition(value); }

        private void SetIncludeTransferCondition(IncludeTransferCondition value)
        {
            if (Conditions.Contains(_IncludeTransferCondition)) Conditions.Remove(_IncludeTransferCondition);
            _IncludeTransferCondition = value;
            if (_IncludeTransferCondition != null) Conditions.Add(_IncludeTransferCondition);
        }
        /// <summary>
        /// Включать переводы или не вклчать
        /// </summary>
        public IncludeTransferCondition IncludeTransfer { get => _IncludeTransferCondition; set => SetIncludeTransferCondition(value); }

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
        }
        public override IEnumerable<T> Apply<T>(IEnumerable<T> srcArr)
        {
            _addAcceptTimeCondition(); //Условие по периоду
            IEnumerable<T> tt = (IEnumerable<T>)srcArr;
            return base.Apply(tt);
        }
    }
}