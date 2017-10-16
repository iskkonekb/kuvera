using System;
using System.Collections.Generic;
using iskkonekb.kuvera.model;
using iskkonekb.kuvera.core;

namespace iskkonekb.kuvera.engine.test
{
    /// <summary>
    /// Плацдарм для тестов
    /// </summary>
    public class BaseTests
    {
        protected Department kitchen = new Department("kitchen");
        protected Project rathayatra = new Project("rathayatra-2017");
        protected Category food = new Category("food", "Продукты питания");
        protected Category initSaldo = new Category(SysCategory.initSaldo.ToString(), "Остаток на момент создания счета");
        protected Category donation = new Category("donation", "Пожертвования");
        protected Account kitchen_cash;
        protected Account kitchen_card;
        protected Engine engine;
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
        public BaseTests()
        {
            InitModel();
            engine = new Engine();
            engine.RegisterEntries(GetEntries());
        }
        protected IEnumerable<IEntry> GetEntries()
        {
            yield return new Entry()
            {
                AcceptTime = EngineConsts.NullDate,
                Plus = true,
                Account = kitchen_cash,
                Category = initSaldo,
                Value = 2000,
                Comment = "Пополнение остатка"
            };
            yield return new Entry()
            {
                AcceptTime = EngineConsts.NullDate,
                Plus = true,
                Account = kitchen_card,
                Category = initSaldo,
                Value = 3000,
                Comment = "Пополнение остатка"
            };
            yield return new Entry()
            {
                AcceptTime = new DateTime(2017, 7, 15),
                Project = rathayatra,
                Plus = false,
                Account = kitchen_card,
                Category = food,
                Value = 1000,
                Comment = "Верный"
            };
            yield return new Entry()
            {
                AcceptTime = new DateTime(2017, 7, 15),
                Project = rathayatra,
                Plus = false,
                Account = kitchen_cash,
                Category = food,
                Value = 1100,
                Comment = "База"
            };
            yield return new Entry()
            {
                AcceptTime = new DateTime(2017, 7, 16),
                Project = rathayatra,
                Account = kitchen_card,
                Plus = false,
                Category = food,
                Value = 500,
                Comment = "Верный"
            };
            yield return new Entry()
            {
                AcceptTime = new DateTime(2017, 7, 17),
                Project = rathayatra,
                Plus = true,
                Account = kitchen_cash,
                Category = food,
                Value = 2100,
                Comment = "unknown payer"
            };
            yield return new Entry()
            {
                AcceptTime = new DateTime(2017, 7, 18),
                Project = rathayatra,
                Plus = true,
                Transfer = true,
                Account = kitchen_card,
                CorrespondAccount = kitchen_cash,
                Value = 2000,
                Collection = "p201709121234",
                Comment = "unknown payer"
            };
            yield return new Entry()
            {
                AcceptTime = new DateTime(2017, 7, 18),
                Project = rathayatra,
                Plus = false,
                Transfer = true,
                Account = kitchen_cash,
                CorrespondAccount = kitchen_card,
                Value = 2000,
                Collection = "p201709121234",
                Comment = "unknown payer"
            };
        }
    }
}
