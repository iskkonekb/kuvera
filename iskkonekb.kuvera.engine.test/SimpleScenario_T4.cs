using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iskkonekb.kuvera.model;
using iskkonekb.kuvera.core;

namespace iskkonekb.kuvera.engine.test
{
    /// <summary>
    /// Воспроизведение сценария из https://github.com/iskkonekb/kuvera/issues/4
    /// </summary>
    [TestClass]
    public class SimpleScenario_T4 : BaseTests
    {
        public SimpleScenario_T4() : base()
        {
        }

        /// <summary>
        /// 7. На 15.07 остаток кэша 900 карты 2000
        /// </summary>
        [TestCategory("Initial Engine. Branch #4"), TestMethod]
        public void T4_CashRest1507_900()
        {
            Assert.AreEqual(900, engine.Sum(QueryBuilder.AccountSaldoOut(kitchen_cash, new DateTime(2017, 7, 15)))); 
        }

        /// <summary>
        /// 7. На 15.07 остаток кэша 900 карты 2000
        /// </summary>
        [TestCategory("Initial Engine. Branch #4"), TestMethod]
        public void T4_CardRest1507_2000()
        {
            Assert.AreEqual(2000, engine.Sum(QueryBuilder.AccountSaldoOut(kitchen_card, new DateTime(2017, 7, 15))));
        }

        /// <summary>
        /// 8. На 16.07 остаток кэша 900 карты 1500
        /// </summary>
        [TestCategory("Initial Engine. Branch #4"), TestMethod]
        public void T4_CashRest1607_900()
        {
            Assert.AreEqual(900, engine.Sum(QueryBuilder.AccountSaldoOut(kitchen_cash, new DateTime(2017, 7, 16))));
        }

        /// <summary>
        /// 8. На 16.07 остаток кэша 900 карты 1500
        /// </summary>
        [TestCategory("Initial Engine. Branch #4"), TestMethod]
        public void T4_CardRest1607_1500()
        {
            Assert.AreEqual(1500, engine.Sum(QueryBuilder.AccountSaldoOut(kitchen_card, new DateTime(2017, 7, 16))));
        }

        /// <summary>
        /// 9. На 17.07 остаток кэша 3000 карты 1500
        /// </summary>
        [TestCategory("Initial Engine. Branch #4"), TestMethod]
        public void T4_CashRest1707_3000()
        {
            Assert.AreEqual(3000, engine.Sum(QueryBuilder.AccountSaldoOut(kitchen_cash, new DateTime(2017, 7, 17))));
        }

        /// <summary>
        /// 9. На 17.07 остаток кэша 3000 карты 1500
        /// </summary>
        [TestCategory("Initial Engine. Branch #4"), TestMethod]
        public void T4_CardRest1707_1500()
        {
            Assert.AreEqual(1500, engine.Sum(QueryBuilder.AccountSaldoOut(kitchen_card, new DateTime(2017, 7, 17))));
        }

        /// <summary>
        /// 10. На 18.07 остаток кэша 1000 карты 3500
        /// </summary>
        [TestCategory("Initial Engine. Branch #4"), TestMethod]
        public void T4_CashRest1807_1000()
        {
            Assert.AreEqual(1000, engine.Sum(QueryBuilder.AccountSaldoOut(kitchen_cash, new DateTime(2017, 7, 18))));
        }

        /// <summary>
        /// 10. На 18.07 остаток кэша 1000 карты 3500
        /// </summary>
        [TestCategory("Initial Engine. Branch #4"), TestMethod]
        public void T4_CardRest1807_3500()
        {
            Assert.AreEqual(3500, engine.Sum(QueryBuilder.AccountSaldoOut(kitchen_card, new DateTime(2017, 7, 18))));
        }

        /// <summary>
        /// 11. Сумма расходов за июль по департаменту 2600
        /// </summary>
        [TestCategory("Initial Engine. Branch #4"), TestMethod]
        public void T4_Outcome_Sum_July_2600()
        {
            Assert.AreEqual(2600, engine.Sum(QueryBuilder.SumDepart(new DateTime(2017, 7, 1),
                new DateTime(2017, 7, 31),
                kitchen,
                false)));
        }

        /// <summary>
        /// 12. Сумма доходов за июль по департаменту 2100
        /// </summary>
        [TestCategory("Initial Engine. Branch #4"), TestMethod]
        public void T4_Income_Sum_July_2100()
        {
            Assert.AreEqual(2100, engine.Sum(QueryBuilder.SumDepart(new DateTime(2017, 7, 1),
                new DateTime(2017, 7, 31),
                kitchen,
                true)));
        }

        /// <summary>
        /// 13. Общая сумма доступных средств департамента на начало месяца 5000
        /// </summary>
        [TestCategory("Initial Engine. Branch #4"), TestMethod]
        public void T4_InRest_July_5000()
        {
            Assert.AreEqual(5000, engine.Sum(QueryBuilder.DepartSaldoOut(kitchen, new DateTime(2017, 7, 1))));
        }

        /// <summary>
        /// 14. Общая сумма доступных средств департамента на конец месяца 4500
        /// </summary>
        [TestCategory("Initial Engine. Branch #4"), TestMethod]
        public void T4_OutRest_July_4500()
        {
            Assert.AreEqual(4500, engine.Sum(QueryBuilder.DepartSaldoOut(kitchen, new DateTime(2017, 7, 31))));
        }
    }
}
