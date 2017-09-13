using System;

namespace iskkonekb.kuvera.model
{
    public class Account
    {
        private string v;

        public Account(string v)
        {
            this.v = v;
        }

        public Department Department { get; set; }
        public DateTime DateCreate { get; set; }
    }
}