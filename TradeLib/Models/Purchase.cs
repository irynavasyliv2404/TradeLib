using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TradeLib.Models
{
    class Purchase
    {
        public string OrderUserName { set; get; }
        public string MatchingOrderUserName { set; get; }
        public double Price { set; get; }
        public TradeType TradeType { set; get; }

        public override string ToString()
        {
            switch (this.TradeType)
            {
                case TradeType.Buy:
                    return $"{OrderUserName} bought a pumpkin from {MatchingOrderUserName} for {Price}€";
                case TradeType.Sell:
                    return $"{OrderUserName} sold a pumpkin to {MatchingOrderUserName} for {Price}€";
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}
