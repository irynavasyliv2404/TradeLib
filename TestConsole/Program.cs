using System;
using TradeLib;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new TradeManager();
            manager.Buy("A", 10);
            manager.Buy("B", 11);
            manager.Sell("C", 15);
            manager.Sell("D", 9);
            manager.Buy("E", 10);
            manager.Sell("F", 10);
            manager.Buy("G", 100);

            var purchases = manager.GetAllPurchases();
            Console.WriteLine("Hello World!");
        }
    }
}
