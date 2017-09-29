using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using iskkonekb.kuvera.model;
using System.Linq;

namespace iskkonekb.kuvera.engine.test
{
    /// <summary>
    /// Тесты для Query
    /// </summary>
    [TestClass]
    public class Refactoring_T6 : BaseTests
    {
        IEnumerable<Entry> entries;
        public Refactoring_T6() : base()
        {
            entries = GetEntries();
        }

        public class RashodCondition : ICondition
        {
            public IEnumerable<T> Apply<T>(IEnumerable<T> query)
            {
                IEnumerable<Entry> tt = (IEnumerable<Entry>)query;
                return (IEnumerable<T>)tt.Where(x => x.Type == EntryType.Outcome);
            }
        }
        public class AccountCondition : ICondition
        {
            Account _acc;
            public AccountCondition(Account acc)
            {
                _acc = acc;
            }
            public IEnumerable<T> Apply<T>(IEnumerable<T> query)
            {
                IEnumerable<Entry> tt = (IEnumerable<Entry>)query;
                return (IEnumerable<T>)tt.Where(x => x.Outcome == _acc);
            }
        }

        [TestMethod]
        public void FilterRashod()
        {
            var q = new Query
            { /*type = QueryTypes.Select,*/
                where = { new RashodCondition() },
                subQ = {
                    new Query { Set = ResultSets.Union, where = { new AccountCondition(kitchen_card) } },
                    new Query { where = { new AccountCondition(kitchen_cash) } }
                }
            };
           Assert.AreEqual(3, q.Filter(entries).Count());
        }
        [TestMethod]
        public void CombinedFilterRashod()
        {
            var q = new Query
            { /*type = QueryTypes.Select,*/
                where = { new RashodCondition() },
                subQ = {
                    new Query { Set = ResultSets.Combined, where = { new AccountCondition(kitchen_card) } }
                }
            };
            Assert.AreEqual(2, q.Filter(entries).Count());
        }
        [TestMethod]
        public void UnionRashod4ParentQ()
        {
            var q = new Query
            {
                where = { new RashodCondition() }
            };
            var q_Cash = new Query { Set = ResultSets.Union, parent = q, where = { new AccountCondition(kitchen_cash) } };
            Assert.AreEqual(1, q_Cash.Filter(entries).Count());
        }
        [TestMethod]
        public void CombineRashodWithParentQ()
        {
            var q = new Query
            {
                where = { new RashodCondition() }
            };
            var q_Card = new Query { Set = ResultSets.Combined, parent = q, where = { new AccountCondition(kitchen_card) } };
            Assert.AreEqual(2, q_Card.Filter(entries).Count());
        }
        [TestMethod]
        public void EntryAcceptDateCondition()
        {
            var q = new EQuery
            {
                AcceptTime = {From = new DateTime(2017, 7, 1), To = new DateTime(2017, 7, 31) }
            };
            Assert.AreEqual(5, q.Filter(entries).Count());
        }
        [TestMethod]
        public void EntryDepartCondition()
        {
            var q = new EQuery
            {
                Department = kitchen,
                Type = EntryType.Outcome,
                AcceptTime = { From = new DateTime(2017, 7, 1), To = new DateTime(2017, 7, 31) }
            };
            Assert.AreEqual(3, q.Filter(entries).Count());
        }
    }
}
