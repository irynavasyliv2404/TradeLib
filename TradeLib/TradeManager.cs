﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLib.Models;

namespace TradeLib
{
    public class TradeManager
    {
        private List<Order> _orders;
        private List<Purchase> _purchases;

        private Func<IEnumerable<Order>, Order, Order> _orderMatchingFilter;

        public TradeManager()
        {
            _orders = new List<Order>();
            _purchases = new List<Purchase>();
        }

        public void Buy(string userName, double price)
        {
            _orderMatchingFilter = BuyMatchFilter;
            MatchOrders(new Order
            {
                UserName = userName,
                Price = price,
                TradeType = TradeType.Buy,
                OrderTime = DateTime.Now
            });
        }

        public void Sell(string userName, double price)
        {
            _orderMatchingFilter = SellMatchFilter;
            MatchOrders(new Order
            {
                UserName = userName,
                Price = price,
                TradeType = TradeType.Sell,
                OrderTime = DateTime.Now
            });
        }

        private void MatchOrders(Order order)
        {
            var matchOrder = _orderMatchingFilter(_orders, order);
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

        private Order BuyMatchFilter(IEnumerable<Order> _orders, Order order)
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

        private Order SellMatchFilter(IEnumerable<Order> _orders, Order order)
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

        public IEnumerable<string> GetAllPurchases()
        {
            return _purchases.Select(c=>c.ToString());
        }
    }
}