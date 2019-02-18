using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TradeLib.Tests
{
    [TestClass]
    public class TradeManagerTests
    {
        private readonly TradeManager _tradeManager = new TradeManager();

        [TestMethod]
        public void Buy_ThrowException_WhenPriceIsNotPositive()
        {
            Action action = () => _tradeManager.Buy("username", -1);
            Assert.ThrowsException<Exception>(action, "Price has to be more than zero");
        }

        [TestMethod]
        public void Sell_ThrowException_WhenPriceIsNotPositive()
        {
            Action action = () => _tradeManager.Sell("username", -1);
            Assert.ThrowsException<Exception>(action, "Price has to be more than zero");
        }

        [TestMethod]
        public void GetAllPurchases()
        {
            var expectedPurchases = new List<string>()
            {
                "D sold a pumpkin to B for 9€",
                "F sold a pumpkin to A for 10€",
                "G bought a pumpkin from C for 100€"
            };

            _tradeManager.Buy("A", 10);
            _tradeManager.Buy("B", 11);
            _tradeManager.Sell("C", 15);
            _tradeManager.Sell("D", 9);
            _tradeManager.Buy("E", 10);
            _tradeManager.Sell("F", 10);
            _tradeManager.Buy("G", 100);

            var actualPurchases = _tradeManager.GetAllPurchases().ToList();
            CollectionAssert.AreEqual(expectedPurchases, actualPurchases);
        }

        [TestMethod]
        public void GetAllPurchases_Multithreading()
        {
            var expectedPurchases = new List<string>()
            {
                "C sold a pumpkin to B for 9€",
                "D sold a pumpkin to A for 8€",
            };

            _tradeManager.Buy("A", 10);
            _tradeManager.Buy("B", 11);

            Thread thread1 = new Thread(() => _tradeManager.Sell("C", 9));
            Thread thread2 = new Thread(() => _tradeManager.Sell("D", 8));

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            var actualPurchases = _tradeManager.GetAllPurchases().ToList();
            CollectionAssert.AreEqual(expectedPurchases, actualPurchases);
        }
    }
}
