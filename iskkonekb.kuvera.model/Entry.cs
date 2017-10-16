using System;
using iskkonekb.kuvera.core;

namespace iskkonekb.kuvera.model
{
    public class Entry : IEntry
    {
        public Entry()
        {
        }
        public DateTime AcceptTime { get; set; }
        public ICategory Category { get; set; }
        public IProject Project { get; set; }
        public string Comment { get; set; }
        public decimal Value { get; set; }
        public IAccount Account { get; set; }
        public IAccount CorrespondAccount { get; set; }
        public string Collection { get; set; }
        public bool Transfer { get; set; }
        public bool Plus { get; set; }
    }
}