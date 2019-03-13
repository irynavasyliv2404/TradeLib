using System;

namespace Tradelib.Library
{
    public class Class
    {
        private int _a;

        public string A(int _a)
        {
            this._a = _a;
            if (_a == 9)
            {
                return "Hello";
            }
            else
            {
                _a += 5;
                return "Lol";
            }
        }
    }
}
