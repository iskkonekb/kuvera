using System;

namespace iskkonekb.kuvera.core
{
    public interface IAccount
    {
        IDepartment Department { get; set; }
        DateTime DateCreate { get; set; }
    }
}