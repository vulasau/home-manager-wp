using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeManager.Tools.Tests
{
    [TestClass]
    public class DepositCalculatorTests
    {
        [TestMethod]
        public void CalculateDepositTest()
        {
            var calculator = new DepositCalculator();
            var info = calculator.CalculateDeposit(1000, 100, 5, 12);
            Assert.AreEqual(info.ClearAmount, 2200);
            Assert.AreEqual(Math.Round(info.TotalAmount), 2279);
            Assert.AreEqual(Math.Round(info.Income), 79);
        }
    }
}
