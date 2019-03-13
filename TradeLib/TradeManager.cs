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
        private readonly List<Order> _orders = new List<Order>();
        private readonly List<Purchase> _purchases = new List<Purchase>();

        public void Buy(string userName, double price)
        {
            var order = ValidateAndCreateOrder(userName, price);
            order.TradeType = TradeType.Buy;
            ManagePurchase(order, GetMatchingSellersForBuyer);
        }

        public void Sell(string userName, double price)
        {
            var order = ValidateAndCreateOrder(userName, price);
            order.TradeType = TradeType.Sell;
            ManagePurchase(order, GetMatchingBuyersForSeller);
        }

        public IEnumerable<string> GetAllPurchases()
        {
            return _purchases.Select(c => c.ToString());
        }

        private Order ValidateAndCreateOrder(string userName, double price)
        {
            if (price <= 0)
            {
                throw new Exception("Price has to be more than zero");
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception("Name cannot be empty");
            }

            return new Order()
            {
                UserName = userName,
                Price = price,
                OrderTime = DateTime.UtcNow
            };
        }

        private void ManagePurchase(Order order, Func<IEnumerable<Order>, Order, IOrderedEnumerable<Order>> findOrderMatch)
        {
            lock (_locker)
            {
                var matchingOrder = findOrderMatch(_orders.Where(c => c.TradeType != order.TradeType), order)
                    .ThenBy(c => c.OrderTime)
                    .FirstOrDefault();

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

                var a = (long)1;

                _orders.Remove(matchingOrder);
            }
        }

        private IOrderedEnumerable<Order> GetMatchingSellersForBuyer(IEnumerable<Order> sellers, Order order)
        {
            return sellers
                .Where(c => c.Price <= order.Price)
                .OrderBy(c => c.Price);
        }

        private IOrderedEnumerable<Order> GetMatchingBuyersForSeller(IEnumerable<Order> buyers, Order order)
        {
            return buyers
                .Where(c => c.Price >= order.Price)
                .OrderByDescending(c => c.Price);
        }
    }
}
