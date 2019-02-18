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
        public void Buy_ThrowException_WhenNameIsNull()
        {
            Action action = () => _tradeManager.Buy(null, -1);
            Assert.ThrowsException<Exception>(action, "Name cannot be empty");
        }

        [TestMethod]
        public void Sell_ThrowException_WhenNameIsNull()
        {
            Action action = () => _tradeManager.Sell(null, -1);
            Assert.ThrowsException<Exception>(action, "Name cannot be empty");
        }

        [TestMethod]
        public void Buy_ThrowException_WhenNameIsEmpty()
        {
            Action action = () => _tradeManager.Buy("", -1);
            Assert.ThrowsException<Exception>(action, "Name cannot be empty");
        }

        [TestMethod]
        public void Sell_ThrowException_WhenNameIsEmpty()
        {
            Action action = () => _tradeManager.Sell("", -1);
            Assert.ThrowsException<Exception>(action, "Name cannot be empty");
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
        public void GetAllPurchases_MultiThreading()
        {
            var expectedPurchases = new List<string>()
            {
                "D sold a pumpkin to B for 9€",
                "E sold a pumpkin to C for 8€"
            };

            _tradeManager.Buy("A", 10);
            _tradeManager.Buy("B", 11);

            Thread thread1 = new Thread(() => _tradeManager.Sell("D", 9));
            Thread thread2 = new Thread(() => _tradeManager.Buy("C", 12));
            Thread thread3 = new Thread(() => _tradeManager.Sell("E", 8));
            
            thread1.Start();
            Thread.Sleep(10);
            thread2.Start();
            Thread.Sleep(10);
            thread3.Start();

            thread1.Join();
            thread2.Join();
            thread3.Join();

            var actualPurchases = _tradeManager.GetAllPurchases().ToList();
            CollectionAssert.AreEquivalent(expectedPurchases, actualPurchases);
        }
    }
}
