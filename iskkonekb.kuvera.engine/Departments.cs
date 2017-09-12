using System;
using System.Collections.Generic;
using iskkonekb.kuvera.model;
using System.Linq;

namespace iskkonekb.kuvera.engine
{
    public class Departments : IDepartments
    {
        private List<Entry> _entries = new List<Entry>();

        public void SetEntries(List<Entry> value) { _entries = value; }

        public Departments()
        {
        }

        /// <summary>
        /// Сумма оборотов по департаменту за период
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="department"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public decimal Sum(DateTime startdate, DateTime enddate, Department department, EntryType type)
        {
            if (_entries.Count == 0) return 0;
            return _entries.Where(it => it.Type == type &&
            it.AcceptTime >= startdate && it.AcceptTime <= enddate
            && (type == EntryType.Income ? it.Income : it.Outcome).Department == department
            ).Sum(it => it.Value);
        }
    }
}