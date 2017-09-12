using System;
using System.Collections.Generic;
using iskkonekb.kuvera.model;
using System.Linq;

namespace iskkonekb.kuvera.engine
{
    public class Entries : IEntries
    {
        private List<Entry> _entries = new List<Entry>();
        public List<Entry> GetEntries()
        {
            return _entries;
        }
        /// <summary>
        /// Провести набор проводок
        /// </summary>
        /// <param name="entries">Проводки ддля проведения</param>
        public void RegisterEntries(IEnumerable<Entry> entries)
        {
            _entries.AddRange(entries);
        }

        /// <summary>
        /// Сохранить в кэш набор проводок
        /// </summary>
        /// <param name="entries">Проводки ддля проведения</param>
        public IEnumerable<Entry> GetEntries(Account account, DateTime startdate, DateTime enddate)
        {
            return _entries.Where(x => x.AcceptTime >= startdate && x.AcceptTime <= enddate
            && x.Income == account
            );
        }

        public Entries()
        {
        }
    }
}