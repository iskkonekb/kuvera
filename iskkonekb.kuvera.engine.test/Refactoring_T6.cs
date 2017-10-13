using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using iskkonekb.kuvera.model;
using iskkonekb.kuvera.model.QueryConditions;
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
        public class DohodCondition : ICondition
        {
            public IEnumerable<T> Apply<T>(IEnumerable<T> query)
            {
                IEnumerable<Entry> tt = (IEnumerable<Entry>)query;
                return (IEnumerable<T>)tt.Where(x => x.Type == EntryType.Income);
            }
        }
        public class AccountConditionSimple : ICondition
        {
            Account _acc;
            public AccountConditionSimple(Account acc)
            {
                _acc = acc;
            }
            public IEnumerable<T> Apply<T>(IEnumerable<T> query)
            {
                IEnumerable<Entry> tt = (IEnumerable<Entry>)query;
                return (IEnumerable<T>)tt.Where(x => x.Outcome == _acc || x.Income == _acc);
            }
        }

        [TestMethod]
        public void T6_FilterParentRashodCard()
        {
            var qParent = new Query { Conditions = { new RashodCondition() } };
            var q = new Query
            {
                Parent = qParent,
                Conditions = { new AccountConditionSimple(kitchen_card) }
            };
            Assert.AreEqual(2, q.Apply(entries).Count());
        }
        [TestMethod]
        public void T6_FilterSelfRashodCard()
        {
            var q = new Query
            {
                Conditions = { new RashodCondition(), new AccountConditionSimple(kitchen_card) }
            };
            Assert.AreEqual(2, q.Apply(entries).Count());
        }

        [TestMethod]
        public void T6_0SumSelfRashodCard()
        {
            var q = new Query
            {
                QueryType = QueryTypes.Sum,
                Conditions = { new RashodCondition(), new AccountConditionSimple(kitchen_card) }
            };
            Assert.AreEqual(0, engine.Sum(q));
        }
        [TestMethod]
        public void T6_1500_SumSelfRashodCard()
        {
            var q = new Query
            {
                QueryType = QueryTypes.Sum,
                SubQueries = {  new Query   {   QueryType = QueryTypes.Primary,
                                                Conditions = { new RashodCondition(), new AccountConditionSimple(kitchen_card) }
                                            }
                            }

            };
            Assert.AreEqual(1500, engine.Sum(q));
        }
        [TestMethod]
        public void T6_1500_SumParentRashodCard()
        {
            var qParent = new Query
            {
                QueryType = QueryTypes.Primary,
                Conditions = { new RashodCondition() }
            };
            var q = new Query
            {
                QueryType = QueryTypes.Sum,
                SubQueries = {  new Query   {   Parent = qParent,
                                                QueryType = QueryTypes.Primary,
                                                Conditions = { new AccountConditionSimple(kitchen_card) }
                                            }
                            }

            };
            Assert.AreEqual(1500, engine.Sum(q));
        }
        [TestMethod]
        public void T6_2600_SumParentRashodAccounts()
        {
            var qParent = new Query
            {
                QueryType = QueryTypes.Primary,
                Conditions = { new RashodCondition() }
            };
            var q = new Query
            {
                QueryType = QueryTypes.Sum,
                SubQueries = {  new Query   {   Parent = qParent,
                                                QueryType = QueryTypes.Primary,
                                                Conditions = { new AccountConditionSimple(kitchen_card) }
                                            },
                                new Query { Parent = qParent,
                                            QueryType = QueryTypes.Primary,
                                            Conditions = { new AccountConditionSimple(kitchen_cash) }
                                }
                            }

            };
            Assert.AreEqual(2600, engine.Sum(q));
        }
        [TestMethod]
        public void T6_400_DeltaCard()
        {
            var qParent = new Query
            {
                QueryType = QueryTypes.Primary,
                Conditions = { new AccountConditionSimple(kitchen_card) }
            };
            var q = new Query
            {
                QueryType = QueryTypes.Sum,
                SubQueries = {  new Query   {   Parent = qParent,
                                                QueryType = QueryTypes.Primary,
                                                Conditions = { new DohodCondition() }
                                            },
                                new Query   {   Parent = qParent,
                                                QueryType = QueryTypes.Primary,
                                                Negate = true,
                                                Conditions = { new RashodCondition() }
                                            }
                            }

            };
            Assert.AreEqual(1500, engine.Sum(q));
        }
        [TestMethod]
        public void T6_Ngate2600_DeltaCard()
        {
            var q = new Query
            {
                QueryType = QueryTypes.Sum,
                SubQueries = {  new Query   {   QueryType = QueryTypes.Sum,
                                                IgnoreMinus = true,
                                                SubQueries = {
                                                                new Query{
                                                                    QueryType = QueryTypes.Primary,
                                                                    Conditions = {new RashodCondition()}
                                                                },
                                                                new Query() {
                                                                    QueryType  = QueryTypes.Primary,
                                                                    Negate =true,
                                                                    Conditions = { new DohodCondition()}
                                                                }
                                                }
                                            },
                                new Query   {   QueryType = QueryTypes.Primary,
                                                Negate = true,
                                                Conditions = { new RashodCondition() }
                                            }
                            }

            };
            Assert.AreEqual(-2600, engine.Sum(q));
        }
        [TestMethod]
        public void T6_EQuery_FilterParentRashod()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Types = new EntryType[] { EntryType.Outcome } },
                EntryType = new EntryType[] { EntryType.Outcome },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
         [TestMethod]
        public void T6_DepCondition()
        {
            var q = new Query
            {
                QueryType = QueryTypes.Primary,
                Conditions = { new DepartmentCondition { Department = kitchen, Types = new EntryType[] { EntryType.Outcome } } }
            };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestMethod]
        public void T6_EQuery_DepCondition()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen},
                EntryType = new EntryType[] { EntryType.Income, EntryType.Outcome}
            };
            Assert.AreEqual(6, q.Apply(entries).Count());
        }
        [TestMethod]
        public void T6_EQuery_IncomeDepCondition()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Types = new EntryType[] { EntryType.Income } },
                EntryType = new EntryType[] { EntryType.Income }
            };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestMethod]
        public void T6_EQuery_AnyDepCondition()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Types = new EntryType[] { EntryType.Any } },
                EntryType = new EntryType[] { EntryType.Any }
            };
            Assert.AreEqual(7, q.Apply(entries).Count());
        }
        [TestMethod]
        public void T6_EQuery_FilterRashod()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Types = new EntryType[] { EntryType.Outcome } },
                EntryType = new EntryType[] { EntryType.Outcome },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestMethod]
        public void T6_EQuery_FilterRashodCard()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Account = new AccountCondition { Account = kitchen_card, Types = new EntryType[] { EntryType.Outcome } },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(2, q.Apply(entries).Count());
        }
        [TestMethod]
        public void T6_EQuery_FilterRashodCardOutcome()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Account = new AccountCondition { Account = kitchen_card, Types = new EntryType[] { EntryType.Outcome } },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            q.Apply(entries);
            q.Account = new AccountCondition { Account = kitchen_cash, Types = new EntryType[] { EntryType.Outcome } };
            Assert.AreEqual(1, q.Apply(entries).Count());
        }
        [TestMethod]
        public void T6_EQuery_FilterRashodCardIncome()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Account = new AccountCondition { Account = kitchen_card, Types = new EntryType[] { EntryType.Income } }
            };
            Assert.AreEqual(1, q.Apply(entries).Count());
        }
        [TestMethod]
        public void T6_EQuery_FilterRashodCardAny()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Account = new AccountCondition { Account = kitchen_card, Types = new EntryType[] { EntryType.Any } }
             };
            Assert.AreEqual(4, q.Apply(entries).Count());
        }
        [TestMethod]
        public void T6_EQuery_FilterRashodCardExTransfer()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Account = new AccountCondition { Account = kitchen_card, Types = new EntryType[] { EntryType.Income, EntryType.Outcome} }
            };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestMethod]
        public void T6_EQuery_FilterRashodCard1()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Types = new EntryType[] { EntryType.Outcome } },
                EntryType = new EntryType[] { EntryType.Outcome },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            q.Apply(entries);
            q.AcceptTime = new DateTimeRange { From = EngineConsts.NullDate, To = EngineConsts.NullDate };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestMethod]
        public void T6_EQuery_FilterRashodCard2()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Types = new EntryType[] { EntryType.Outcome } },
                EntryType = new EntryType[] { EntryType.Outcome },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            q.Apply(entries);
            q.Department = null;
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestMethod]
        public void T6_EQuery_FilterDohodCard()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Types = new EntryType[] { EntryType.Income } },
                EntryType = new EntryType[] { EntryType.Income },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(1, q.Apply(entries).Count());
        }
        [TestMethod]
        public void T6_EQuery_FilterDohodCard1()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Types = new EntryType[] { EntryType.Income } },
                EntryType = new EntryType[] { EntryType.Income },
                AcceptTime = new DateTimeRange
                {
                    From = EngineConsts.NullDate,
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void T6_EngineF_Formula()
        {
            var q = new Query
            {
                QueryType = QueryTypes.Formula,
                Conditions = { new DepartmentCondition { Department = kitchen, Types = new EntryType[] { EntryType.Outcome } } }
            };
            var ret = engine.Sum(q);
            Assert.Fail("An exception should have been thrown");
        }
        [TestMethod]
        public void T6_EQuery_AcceptTime()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Types = new EntryType[] { EntryType.Income } },
                EntryType = new EntryType[] { EntryType.Income },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(new System.TimeSpan(2, 0, 0, 0), q.AcceptTime.To - q.AcceptTime.From);
        }
        [TestMethod]
        public void T6_EQuery_EntryType()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Types = new EntryType[] { EntryType.Income } },
                EntryType = new EntryType[] { EntryType.Income },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(1, q.EntryType.Count());
        }
        [TestMethod]
        public void T6_EQuery_Department()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Types = new EntryType[] { EntryType.Income } },
                EntryType = new EntryType[] { EntryType.Income },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(1, q.Department.Types.Count());
        }
        [TestMethod]
        public void T6_EQuery_AccountCondition()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Account = new AccountCondition { Account = kitchen_card, Types = new EntryType[] { EntryType.Outcome } },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(1, q.Account.Types.Count());
        }
        [TestMethod]
        public void T6_EQuery_SetEntryType()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Types = new EntryType[] { EntryType.Income } },
                EntryType = new EntryType[] { EntryType.Income },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(1, q.Department.Types.Count());
        }
    }
}
