﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using iskkonekb.kuvera.model;
using System.Linq;

namespace iskkonekb.kuvera.engine.test
{
    /// <summary>
    /// Воспроизведение сценария из https://github.com/iskkonekb/kuvera/issues/4
    /// </summary>
    [TestClass]
    public class SimpleScenario_T4
    {
        Department kitchen = new Department("kitchen");
        Project rathayatra = new Project("rathayatra-2017");
        Category food = new Category("food", "Продукты питания");
        Category initSaldo = new Category(SysCategory.initSaldo.ToString(), "Остаток на момент создания счета");
        Category donation = new Category("donation", "Пожертвования");
        Account kitchen_cash;
        Account kitchen_card;
        Engine engine;
        private void InitModel()
        {
            kitchen_cash = new Account("kitchen_cash")
            {
                Department = kitchen,
                DateCreate = new DateTime(2017, 1, 1)
            };
            kitchen_card = new Account("kitchen_card")
            {
                Department = kitchen,
                DateCreate = new DateTime(2017, 1, 1)
            };
        }

        private IEnumerable<Entry> GetEntries()
        {
            yield return new Entry()
            {
                AcceptTime = EngineConsts.NullDate,
                Type = EntryType.Income,
                Income = kitchen_cash,
                Category = initSaldo,
                Value = 2000,
                Comment = "Пополнение остатка"
            };
            yield return new Entry()
            {
                AcceptTime = EngineConsts.NullDate,
                Type = EntryType.Income,
                Income = kitchen_card,
                Category = initSaldo,
                Value = 3000,
                Comment = "Пополнение остатка"
            };
            yield return new Entry()
            {
                AcceptTime = new DateTime(2017, 7, 15),
                Project = rathayatra,
                Type=EntryType.Outcome,
                Outcome = kitchen_card,
                Category = food,
                Value = 1000,
                Comment = "Верный"
            };
            yield return new Entry()
            {
                AcceptTime = new DateTime(2017, 7, 15),
                Project = rathayatra,
                Type= EntryType.Outcome,
                Outcome = kitchen_cash,
                Category = food,
                Value = 1100,
                Comment = "База"
            };
            yield return new Entry()
            {
                AcceptTime = new DateTime(2017, 7, 16),
                Project = rathayatra,
                Outcome = kitchen_card,
                Type = EntryType.Outcome,
                Category = food,
                Value = 500,
                Comment = "Верный"
            };
            yield return new Entry()
            {
                AcceptTime = new DateTime(2017, 7, 17),
                Project = rathayatra,
                Type = EntryType.Income,
                Income = kitchen_cash,
                Category = food,
                Value = 2100,
                Comment = "unknown payer"
            };
            yield return new Entry()
            {
                AcceptTime = new DateTime(2017, 7, 18),
                Project = rathayatra,
                Type = EntryType.Transfer,
                Income = kitchen_card,
                Outcome = kitchen_cash,
                Value = 2000,
                Comment = "unknown payer"
            };
        }

        public SimpleScenario_T4()
        {
            InitModel();
            engine = new Engine();
            engine.RegisterEntries(GetEntries());
        }

        /// <summary>
        /// 7. На 15.07 остаток кэша 900 карты 2000
        /// </summary>
        [TestMethod]
        public void CashRest1507_900()
        {
            Assert.AreEqual(900, engine.SaldoOut(kitchen_cash, new DateTime(2017, 7, 15))); 
        }

        /// <summary>
        /// 7. На 15.07 остаток кэша 900 карты 2000
        /// </summary>
        [TestMethod]
        public void CardRest1507_2000()
        {
            Assert.AreEqual(2000, engine.SaldoOut(kitchen_card, new DateTime(2017, 7, 15)));
        }

        /// <summary>
        /// 8. На 16.07 остаток кэша 900 карты 1500
        /// </summary>
        [TestMethod]
        public void CashRest1607_900()
        {
            Assert.AreEqual(900, engine.SaldoOut(kitchen_cash, new DateTime(2017, 7, 16)));
        }

        /// <summary>
        /// 8. На 16.07 остаток кэша 900 карты 1500
        /// </summary>
        [TestMethod]
        public void CardRest1607_1500()
        {
            Assert.AreEqual(1500, engine.SaldoOut(kitchen_card, new DateTime(2017, 7, 16)));
        }

        /// <summary>
        /// 9. На 17.07 остаток кэша 3000 карты 1500
        /// </summary>
        [TestMethod]
        public void CashRest1707_3000()
        {
            Assert.AreEqual(3000, engine.SaldoOut(kitchen_cash, new DateTime(2017, 7, 17)));
        }

        /// <summary>
        /// 9. На 17.07 остаток кэша 3000 карты 1500
        /// </summary>
        [TestMethod]
        public void CardRest1707_1500()
        {
            Assert.AreEqual(1500, engine.SaldoOut(kitchen_card, new DateTime(2017, 7, 17)));
        }

        /// <summary>
        /// 10. На 18.07 остаток кэша 1000 карты 3500
        /// </summary>
        [TestMethod]
        public void CashRest1807_1000()
        {
            Assert.AreEqual(1000, engine.SaldoOut(kitchen_cash, new DateTime(2017, 7, 18)));
        }

        /// <summary>
        /// 10. На 18.07 остаток кэша 1000 карты 3500
        /// </summary>
        [TestMethod]
        public void CardRest1807_3500()
        {
            Assert.AreEqual(3500, engine.SaldoOut(kitchen_card, new DateTime(2017, 7, 18)));
        }

        /// <summary>
        /// 11. Сумма расходов за июль по департаменту 2600
        /// </summary>
        [TestMethod]
        public void Outcome_Sum_July_2600()
        {
            Assert.AreEqual(2600, engine.Sum(new DateTime(2017, 7, 1),
                new DateTime(2017, 7, 31),
                kitchen,
                EntryType.Outcome));
        }

        /// <summary>
        /// 12. Сумма доходов за июль по департаменту 2100
        /// </summary>
        [TestMethod]
        public void Income_Sum_July_2100()
        {
            Assert.AreEqual(2100, engine.Sum(new DateTime(2017, 7, 1),
                new DateTime(2017, 7, 31),
                kitchen,
                EntryType.Income));
        }

        /// <summary>
        /// 13. Общая сумма доступных средств департамента на начало месяца 5000
        /// </summary>
        [TestMethod]
        public void InRest_July_5000()
        {
            Assert.AreEqual(5000, engine.SaldoOut(new DateTime(2017, 7, 1), kitchen));
        }

        /// <summary>
        /// 14. Общая сумма доступных средств департамента на конец месяца 4500
        /// </summary>
        [TestMethod]
        public void OutRest_July_4500()
        {
            Assert.AreEqual(4500, engine.SaldoOut(new DateTime(2017, 7, 31), kitchen));
        }
    }
}
