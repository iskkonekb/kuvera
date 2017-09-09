using System;

namespace iskkonekb.kuvera.model
{
    public class Entry
    {
        public Entry()
        {
        }

        public DateTime AcceptTime { get;  set; }
        public Category Category { get;  set; }
        public EntryType Type { get;  set; }
        public Account Outcome { get;  set; }
        public Project Project { get;  set; }
        public string Comment { get;  set; }
        public decimal Value { get;  set; }
        public Account Income { get;  set; }
    }
}