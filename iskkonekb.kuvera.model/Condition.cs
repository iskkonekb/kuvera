using System.Collections.Generic;

namespace iskkonekb.kuvera.model
{
    public interface ICondition
    {
        IEnumerable<T> Apply<T>(IEnumerable<T> query);
    }
    public class Condition : ICondition
    {
        public Condition()
        {
        }
        public IEnumerable<T> Apply<T>(IEnumerable<T> query)
        {
            throw new System.NotImplementedException();
        }
    }
}