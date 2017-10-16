using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using iskkonekb.kuvera.model;
using iskkonekb.kuvera.core;
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
        IEnumerable<IEntry> entries;
        public Refactoring_T6() : base()
        {
            entries = GetEntries();
        }

        public class RashodCondition : ICondition
        {
            public IEnumerable<T> Apply<T>(IEnumerable<T> query)
            {
                IEnumerable<IEntry> tt = (IEnumerable<IEntry>)query;
                return (IEnumerable<T>)tt.Where(x => x.Plus == false && x.Transfer == false);
            }
        }
        public class DohodCondition : ICondition
        {
            public IEnumerable<T> Apply<T>(IEnumerable<T> query)
            {
                IEnumerable<IEntry> tt = (IEnumerable<IEntry>)query;
                return (IEnumerable<T>)tt.Where(x => x.Plus == true && x.Transfer == false);
            }
        }
        public class AccountConditionSimple : ICondition
        {
            IAccount _acc;
            public AccountConditionSimple(IAccount acc)
            {
                _acc = acc;
            }
            public IEnumerable<T> Apply<T>(IEnumerable<T> query)
            {
                IEnumerable<IEntry> tt = (IEnumerable<IEntry>)query;
                return (IEnumerable<T>)tt.Where(x => x.Account == _acc && x.Transfer == false);
            }
        }

        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
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
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_FilterSelfRashodCard()
        {
            var q = new Query
            {
                Conditions = { new RashodCondition(), new AccountConditionSimple(kitchen_card) }
            };
            Assert.AreEqual(2, q.Apply(entries).Count());
        }

        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_0SumSelfRashodCard()
        {
            var q = new Query
            {
                QueryType = QueryTypes.Sum,
                Conditions = { new RashodCondition(), new AccountConditionSimple(kitchen_card) }
            };
            Assert.AreEqual(0, engine.Sum(q));
        }

        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_AccontConditionGet()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Sum,
                Account = new AccountCondition { Account = kitchen_card }
            };
            Assert.AreEqual(kitchen_card, q.Account.Account);
        }

        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_AccontConditionNull()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Sum,
                Account = new AccountCondition { Account = kitchen_card }
            };
            q.Account = null;
            Assert.AreEqual(null, q.Account);
        }

        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_DepartmentConditionGet()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Sum,
                Department = new DepartmentCondition { Department = kitchen }
            };
            Assert.AreEqual(kitchen, q.Department.Department);
        }

        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_IncludeTransferConditionGet()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Sum,
                IncludeTransfer = new IncludeTransferCondition { IncludeTransfer = true }
            };
            Assert.AreEqual(true, q.IncludeTransfer.IncludeTransfer);
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_IncludeTransferConditionSet()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Sum,
                IncludeTransfer = new IncludeTransferCondition { IncludeTransfer = true }
            };
            q.IncludeTransfer = new IncludeTransferCondition { IncludeTransfer = false };
            Assert.AreEqual(false, q.IncludeTransfer.IncludeTransfer);
        }

        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_PlusConditionGet()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Sum,
                Plus = new EntryPlusCondition { Plus = true }
            };
            Assert.AreEqual(true, q.Plus.Plus);
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_PlusConditionSet()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Sum,
                Plus = new EntryPlusCondition { Plus = true }
            };
            q.Plus = new EntryPlusCondition { Plus = false };
            Assert.AreEqual(false, q.Plus.Plus);
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_PlusConditionSetNull()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Sum,
                Plus = new EntryPlusCondition { Plus = true }
            };
            q.Plus = null;
            Assert.AreEqual(null, q.Plus);
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_IncludeTransferConditionSetNull()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Sum,
                IncludeTransfer = new IncludeTransferCondition { IncludeTransfer = true }
            };
            q.IncludeTransfer = null;
            Assert.AreEqual(null, q.IncludeTransfer);
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
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
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
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
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
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
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
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
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
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
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_FilterParentRashod()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Plus = false },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_AcceptTime()
        {
            var q = new EQuery
            {
                AcceptTime = new DateTimeRange
                {
                    From = EngineConsts.NullDate,
                    To = new DateTime(2017, 7, 17)
                }
            };
            q.AcceptTime = new DateTimeRange { From = new DateTime(2017, 7, 15), To = new DateTime(2017, 7, 17) };
            Assert.AreEqual(new System.TimeSpan(2, 0, 0, 0), q.AcceptTime.To - q.AcceptTime.From);
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_AcceptTimeChoose()
        {
            var q = new EQuery
            {
                AcceptTime = new DateTimeRange
                {
                    From = EngineConsts.NullDate,
                    To = new DateTime(2017, 7, 17)
                }
            };
            q.Apply(entries);
            q.AcceptTime = new DateTimeRange { From = new DateTime(2017, 7, 15), To = new DateTime(2017, 7, 17) };
            IEnumerable<IEntry> tt = q.Apply(entries);
            Assert.AreEqual(4, tt.Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_AcceptTime1()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Plus = true },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(new System.TimeSpan(2, 0, 0, 0), q.AcceptTime.To - q.AcceptTime.From);
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_DepCondition()
        {
            var q = new Query
            {
                QueryType = QueryTypes.Primary,
                Conditions = { new DepartmentCondition { Department = kitchen, Plus = false, IncludeTransfer = false } }
            };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_DepCondition()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { IncludeTransfer = false, Department = kitchen }
            };
            Assert.AreEqual(6, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_IncomeDepCondition()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Plus = true, IncludeTransfer = false }
            };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_AnyDepCondition()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, IncludeTransfer = true },
            };
            Assert.AreEqual(7, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_FilterRashod()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Plus = false },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_FilterRashodCard()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Account = new AccountCondition { Account = kitchen_card, Plus = false },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(2, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_FilterRashodCardOutcome()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Account = new AccountCondition { Account = kitchen_card, Plus = false },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            q.Apply(entries);
            q.Account = new AccountCondition { Account = kitchen_cash, Plus = false };
            Assert.AreEqual(1, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_FilterRashodCardIncome()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Account = new AccountCondition { Account = kitchen_card, Plus = true, IncludeTransfer = false }
            };
            Assert.AreEqual(1, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_FilterRashodCardAny()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Account = new AccountCondition { Account = kitchen_card, IncludeTransfer = true }
            };
            Assert.AreEqual(4, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_FilterRashodCardExTransfer()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Account = new AccountCondition { IncludeTransfer = false, Account = kitchen_card }
            };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_FilterRashodCard1()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Plus = false, IncludeTransfer = false },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            q.Apply(entries);
            q.AcceptTime = new DateTimeRange { From = EngineConsts.NullDate, To = new DateTime(2017, 7, 18) };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_FilterRashodCard2()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Plus = false, IncludeTransfer = false },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            q.Apply(entries);
            q.AcceptTime = new DateTimeRange { From = EngineConsts.NullDate, To = EngineConsts.NullDate };
            Assert.AreEqual(0, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_FilterRashodKitchen()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Plus = false, IncludeTransfer = false },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            q.Apply(entries);
            q.Department = null;
            q.IncludeTransfer = new IncludeTransferCondition { IncludeTransfer = false };
            q.Plus = new EntryPlusCondition { Plus = false };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_FilterRashodKitchen1()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Plus = false, IncludeTransfer = true },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 18)
                }
            };
            Assert.AreEqual(4, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_EntryPlusNull()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Plus = false, IncludeTransfer = false }
            };
            q.Plus = new EntryPlusCondition { Plus = null };
            q.Department.Plus = null;
            IEnumerable<IEntry> tt = q.Apply(entries);
           Assert.AreEqual(6, q.Apply(tt).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_FilterDohodCard()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Plus = true },
                AcceptTime = new DateTimeRange
                {
                    From = new DateTime(2017, 7, 15),
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(1, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        public void T6_EQuery_FilterDohodCard1()
        {
            var q = new EQuery
            {
                QueryType = QueryTypes.Primary,
                Department = new DepartmentCondition { Department = kitchen, Plus = true },
                AcceptTime = new DateTimeRange
                {
                    From = EngineConsts.NullDate,
                    To = new DateTime(2017, 7, 17)
                }
            };
            Assert.AreEqual(3, q.Apply(entries).Count());
        }
        [TestCategory("Refactor Engine. Branch #6"), TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void T6_EngineF_Formula()
        {
            var q = new Query
            {
                QueryType = QueryTypes.Formula,
                Conditions = { new DepartmentCondition { Department = kitchen, Plus = false } }
            };
            var ret = engine.Sum(q);
            Assert.Fail("An exception should have been thrown");
        }
    }
}
