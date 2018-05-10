using System;
using NUnit.Framework;
using SearchDublicates;
//using NUnit.VisualStudio.TestAdapter;

namespace Test
{
    [TestFixture]
    public class UnitTestSearchDublicates
    {
        [Test]
        public void TestMethod1()
        {
            Program cl = new SearchDublicates();
            var tolerance = 0.01;
            double testSum = 2 + 6;
            double realSum = cl.MethodSum(2, 6);
            Assert.AreEqual(testSum, realSum, tolerance);
        }
    }
}
