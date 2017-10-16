using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iskkonekb.kuvera.core;

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
        [TestCategory("Master. Initial tests"), TestMethod]
        public void CreateOutcomeEntry()
        {
            var entry = new Entry
            {
                AcceptTime = new DateTime(2017, 7, 15),
                Category = new Category("food","Продукты питания"),
                Plus = false,
                Account = new Account("kitchen-card"),
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
        [TestCategory("Master. Initial tests"), TestMethod]
        public void CreateIncomeEntry()
        {
            var entry = new Entry
            {
                AcceptTime = new DateTime(2017, 7, 17),
                Category = new Category("donation", "Пожертвование"),
                Plus = false,
                Account = new Account("kitchen-cash"),
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
        [TestCategory("Master. Initial tests"), TestMethod]
        public void CreateTransferEntry()
        {
            var entry = new Entry
            {
                AcceptTime = new DateTime(2017, 7, 18),
                Plus = true,
                Transfer = true,
                CorrespondAccount = new Account("kitchen-cash"),
                Account = new Account("kitchen-card"),
                Value = 2000m
            };
        }
    }
}
