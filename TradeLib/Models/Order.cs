using System;
using System.Collections.Generic;
using System.Text;

namespace TradeLib.Models
{
    public class Order
    {
        public string UserName { set; get; }
        public double Price { set; get; }
        public DateTime OrderTime { set; get; }
        public TradeType TradeType { set; get; }
    }
}
