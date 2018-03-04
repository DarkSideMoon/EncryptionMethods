using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RSA.Library
{
    public static class BigIntegerExtension
    {
        /// <summary>
        /// Method for chek is prime number or not
        /// Work very long time to calculate is prime or not
        /// TODO: Need to rewrite to be faster
        /// </summary>
        /// <param name="value"></param>
        /// <param name="witnesses"></param>
        /// <returns></returns>
        public static bool IsProbablyPrime(this BigInteger value, int witnesses = 10)
        {
            int s = 0;

            if (value <= 1)
                return false;

            if (witnesses <= 0)
                witnesses = 10;

            BigInteger d = value - 1;
            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            Byte[] bytes = new Byte[value.ToByteArray().LongLength];

            for (int i = 0; i < witnesses; i++)
            {
                BigInteger a;
                do
                {
                    new Random().NextBytes(bytes);

                    a = new BigInteger(bytes);
                }
                while (a < 2 || a >= value - 2);

                BigInteger x = BigInteger.ModPow(a, d, value);
                if (x == 1 || x == value - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, value);

                    if (x == 1)
                        return false;
                    if (x == value - 1)
                        break;
                }

                if (x != value - 1)
                    return false;
            }

            return true;
        }
    }
}
