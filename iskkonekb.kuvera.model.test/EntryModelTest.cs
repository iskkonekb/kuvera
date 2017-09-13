using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iskkonekb.kuvera.model.test
{

    /// <summary>
    /// Тесты по проверке модели (в соответствии https://github.com/iskkonekb/kuvera/issues/4)
    /// </summary>
    [TestClass]
    public class EntryModelTest
    {
        /*
         accept_time = 15.07
        category = [food]
        type = Outcome
        outcome  = [kitchen-card]
        project = [rathayatra-2017]
        comment = "Верный"
        value = 1000
        */
        [TestMethod]
        public void CreateOutcomeEntry()
        {
            var entry = new Entry
            {
                AcceptTime = new DateTime(2017, 7, 15),
                Category = new Category("food","Продукты питания"),
                Type = EntryType.Outcome,
                Outcome = new Account("kitchen-card"),
                Project = new Project("rathayatra-2017"),
                Comment = "В Верном",
                Value = 1000m
            };
        }

        /*
        accept_time = 17.07
        category = [donation]
        type = Income
        income  = [kitchen-cash]
        comment = "от физлиа"
        value = 2100
        */
        [TestMethod]
        public void CreateIncomeEntry()
        {
            var entry = new Entry
            {
                AcceptTime = new DateTime(2017, 7, 17),
                Category = new Category("donation", "Пожертвование"),
                Type = EntryType.Income,
                Outcome = new Account("kitchen-cash"),
                Comment = "от физлица",
                Value = 2100m
            };
        }
        /*
         * accept_time = 18.07
type = Transfer
income = [kitchen-card]
outcome = [kitchen-cash]
value = 2000
*/
        [TestMethod]
        public void CreateTransferEntry()
        {
            var entry = new Entry
            {
                AcceptTime = new DateTime(2017, 7, 18),
                Type = EntryType.Transfer,
                Outcome = new Account("kitchen-cash"),
                Income = new Account("kitchen-card"),
                Value = 2000m
            };
        }
    }
}
