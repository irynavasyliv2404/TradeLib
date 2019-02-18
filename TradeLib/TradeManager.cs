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

        public TradeManager()
        {
            _orders = new List<Order>();
            _purchases = new List<Purchase>();
        }

        public void Buy(string userName, double price)
        {
            if (price <= 0)
                throw new Exception("Price has to be more than zero");

            ManagePurchase(new Order
            {
                UserName = userName,
                Price = price,
                TradeType = TradeType.Buy,
                OrderTime = DateTime.UtcNow
            }, FindSellerForBuyer);
        }

        public void Sell(string userName, double price)
        {
            if (price <= 0)
                throw new Exception("Price has to be more than zero");

            ManagePurchase(new Order
            {
                UserName = userName,
                Price = price,
                TradeType = TradeType.Sell,
                OrderTime = DateTime.UtcNow
            }, FindBuyerForSeller);
        }

        public IEnumerable<string> GetAllPurchases()
        {
            return _purchases.Select(c => c.ToString());
        }

        private void ManagePurchase(Order order, Func<Order, Order> findOrderMatch)
        {
            lock (_locker)
            {
                var matchingOrder = findOrderMatch(order);
                if (matchingOrder == null)
                {
                    _orders.Add(order);
                    return;
                }
                _purchases.Add(new Purchase()
                {
                    TradeType = order.TradeType,
                    Price = order.Price,
                    OrderUserName = order.UserName,
                    MatchingOrderUserName = matchingOrder.UserName
                });

                _orders.Remove(matchingOrder);
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
