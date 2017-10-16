using System.Collections.Generic;
using System.Linq;
using iskkonekb.kuvera.core;

namespace iskkonekb.kuvera.model.QueryConditions
{
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
            IEnumerable<IEntry> tt = (IEnumerable<IEntry>)query;
            return (IEnumerable<T>)tt.Where(x => (x.AcceptTime >= _AcceptTime.From || _AcceptTime.From == EngineConsts.NullDate)
                    && x.AcceptTime <= _AcceptTime.To);
        }
    }
}