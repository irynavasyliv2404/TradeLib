using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TradeLib.Models;

namespace TradeLib
{
    public class TradeManager
    {
        private readonly object _locker = new object();

        private readonly List<Order> _orders;
        private readonly List<Purchase> _purchases;

        private Func<Order, Order> _findOrderMatch;

        public TradeManager()
        {
            _orders = new List<Order>();
            _purchases = new List<Purchase>();
        }

        public void Buy(string userName, double price)
        {
            if (price <= 0)
                throw new Exception("Price has to be more than zero");
            _findOrderMatch = FindSellerForBuyer;
            ManagePurchase(new Order
            {
                UserName = userName,
                Price = price,
                TradeType = TradeType.Buy,
                OrderTime = DateTime.Now
            });
        }

        public void Sell(string userName, double price)
        {
            if (price <= 0)
                throw new Exception("Price has to be more than zero");
            _findOrderMatch = FindBuyerForSeller;
            ManagePurchase(new Order
            {
                UserName = userName,
                Price = price,
                TradeType = TradeType.Sell,
                OrderTime = DateTime.Now
            });
        }

        public IEnumerable<string> GetAllPurchases()
        {
            return _purchases.Select(c => c.ToString());
        }

        private void ManagePurchase(Order order)
        {
            lock (_locker)
            {
                var matchOrder = _findOrderMatch(order);
                if (matchOrder == null)
                {
                    _orders.Add(order);
                    return;
                }
                _purchases.Add(new Purchase()
                {
                    TradeType = order.TradeType,
                    Price = order.Price,
                    OrderUserName = order.UserName,
                    MathingOrderUserName = matchOrder.UserName
                });

                _orders.Remove(matchOrder);
            }
        }

        private Order FindSellerForBuyer(Order order)
        {
            var matchingOrders = _orders.Where(c => c.TradeType != order.TradeType && c.Price <= order.Price).ToList();

            if (!matchingOrders.Any())
                return null;

            var resultMatch = matchingOrders
                .OrderBy(c => c.Price)
                .ThenBy(c => c.OrderTime)
                .FirstOrDefault();

            return resultMatch;
        }

        private Order FindBuyerForSeller(Order order)
        {
            var matchingOrders = _orders.Where(c => c.TradeType != order.TradeType && c.Price >= order.Price).ToList();

            if (!matchingOrders.Any())
                return null;

            var resultMatch = matchingOrders
                .OrderByDescending(c => c.Price)
                .ThenBy(c => c.OrderTime)
                .FirstOrDefault();

            return resultMatch;
        }
    }
}
