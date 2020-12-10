using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4
{
    public class KeyGenerator
    {
        private long exp = 0, dxp = 0, n = 0;

        private Random random = new Random();

        private readonly long[] F_PRIME_NUMBERS =
        {
            17, 257, 65537
        };

        public KeyGenerator()
        {
            GenerateKeys();
        }

        private bool isPrime(int number)
        {
            if (number == 1) return false;
            if (number == 2) return true;

            var limit = Math.Ceiling(Math.Sqrt(number));

            for (int i = 2; i <= limit; ++i)
                if (number % i == 0)
                    return false;
            return true;

        }
        private int GetRandomPrimeNum()
        {
            var r = new Random();
            int num = r.Next(100, 1000);

            while (!isPrime(num))
                num = r.Next(100, 1000);

            return num;
        }

        public void GenerateKeys()
        {
            long p = GetRandomPrimeNum();
            long q = GetRandomPrimeNum();

            n = p * q;
            long f = LCM((p - 1), (q - 1));

            //exp = F_PRIME_NUMBERS[random.Next(F_PRIME_NUMBERS.Length)];
            exp = GenerateOpenKey(f);
            dxp = ModInverse(exp, f);
        }

        public long GenerateOpenKey(long fi)
        {
            long nod = 0;
            long num = -1;

            while (nod != 1)
            {
                num = random.Next(2, (int)fi);
                nod = GCD(fi, num);
            }

            return num;
        }

        public (long, long) PublicKey
        {
            get { return (exp, n); }
        }

        public (long, long) PrivateKey
        {
            get { return (dxp, n); }
        }

        // НОД двух чисел (аллгоритм Евклида)
        long GCD(long a, long b)
        {
            while (true)
            {
                if (a == 0) return b;
                b %= a;
                if (b == 0) return a;
                a %= b;
            }
        }

        // Наименьшее общее кратное
        long LCM(long a, long b)
        {
            long temp = GCD(a, b);
            return temp != 0 ? (a / temp * b) : 0;
        }

        // обратный по модулю (Расширенный алгоритм Евклида)
        public long ModInverse(long e, long fn)
        {
            long d = 1;
            while (true)
            {
                if ((e * d) % fn == 1)
                    break;
                else
                    d++;
            }
            return d;
        }
    }
}
