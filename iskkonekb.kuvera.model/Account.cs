using System;
using iskkonekb.kuvera.core;
namespace iskkonekb.kuvera.model
{
    public class Account : IAccount
    {
        private string v;

        public Account(string v)
        {
            this.v = v;
        }

        public IDepartment Department { get; set; }
        public DateTime DateCreate { get; set; }
    }
}