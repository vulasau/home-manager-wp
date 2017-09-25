using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeManager.Tools.Tests
{
    [TestClass]
    public class CreditCalculatorTests
    {
        [TestMethod]
        public void CalculateCreditTest()
        {
            var creditCalculator = new CreditCalculator();
            var creditInfo = creditCalculator.Calculate(1200, 0, 10, 12, 200);
            Assert.AreEqual(Math.Round(creditInfo.PaymentMonth, 0), 106);
            Assert.AreEqual(Math.Round(creditInfo.FullPrice, 0), 1266);
            Assert.IsTrue(creditInfo.IsPossible);

            creditInfo = creditCalculator.Calculate(1200, 0, 10, 12, 100);
            Assert.IsFalse(creditInfo.IsPossible);
        }
    }
}
