using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iskkonekb.kuvera.model.test
{
    [TestClass]
    public class SampleTest
    {
        [TestMethod]
        public void testPublicMethod()
        {
            //un break test
            Assert.AreEqual(2, new SampleClass().publicData());
        }

        [TestMethod]
        public void testInternalMethod()
        {
            Assert.AreEqual(3, new SampleClass().forTestData());
        }
    }
}
