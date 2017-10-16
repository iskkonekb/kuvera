using System;

namespace iskkonekb.kuvera.core
{
    public interface IEntry
    {
        DateTime AcceptTime { get; set; }
        ICategory Category { get; set; }
        IProject Project { get; set; }
        string Comment { get; set; }
        decimal Value { get; set; }
        IAccount Account { get; set; }
        IAccount CorrespondAccount { get; set; }
        string Collection { get; set; }
        bool Transfer { get; set; }
        bool Plus { get; set; }
    }
}