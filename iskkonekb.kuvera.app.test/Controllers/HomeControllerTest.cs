using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iskkonekb.kuvera.app;
using iskkonekb.kuvera.app.Controllers;

namespace iskkonekb.kuvera.app.test.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Упорядочение
            HomeController controller = new HomeController();

            // Действие
            ViewResult result = controller.Index() as ViewResult;

            // Утверждение
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
