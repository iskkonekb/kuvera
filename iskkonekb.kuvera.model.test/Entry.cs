using System;

namespace iskkonekb.kuvera.model.test
{
    internal class Entry
    {
        public Entry()
        {
        }

        public DateTime AcceptTime { get; internal set; }
        public Category Category { get; internal set; }
        public object Type { get; internal set; }
        public Account Outcome { get; internal set; }
        public Project Project { get; internal set; }
        public string Comment { get; internal set; }
        public decimal Value { get; internal set; }
        public Account Income { get; internal set; }
    }
}