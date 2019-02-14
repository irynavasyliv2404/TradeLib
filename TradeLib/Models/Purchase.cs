using System;
using System.Collections.Generic;
using System.Text;

namespace TradeLib.Models
{
    public class Purchase
    {
        public string BuyerUserName { set; get; }
        public string SellerUserName { set; get; }
        public double Price { set; get; }
        public TradeType TradeType { set; get; }
    }
}
