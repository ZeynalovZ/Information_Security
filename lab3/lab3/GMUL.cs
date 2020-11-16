using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace lab3
{
    public class Matrix
    {

        public Matrix()
        {

        }
        public static byte[,] ss = new byte[4, 4];
        //public static byte[,] s = new byte[4, 4];

        private byte GMul(byte a, byte b)
        { // Galois Field (256) Multiplication of two Bytes
            byte p = 0;

            for (int counter = 0; counter < 8; counter++)
            {
                if ((b & 1) != 0)
                {
                    p ^= a;
                }

                bool hi_bit_set = (a & 0x80) != 0;
                a <<= 1;
                if (hi_bit_set)
                {
                    a ^= 0x1B; /* x^8 + x^4 + x^3 + x + 1 */
                }
                b >>= 1;
            }

            return p;
        }

        public void MixColumns(byte[,] s)
        { // 's' is the main State matrix, 'ss' is a temp matrix of the same dimensions as 's'.
            Array.Clear(ss, 0, ss.Length);

            for (int c = 0; c < 4; c++)
            {
                ss[0, c] = (byte)(GMul(0x02, s[0, c]) ^ GMul(0x03, s[1, c]) ^ s[2, c] ^ s[3, c]);
                ss[1, c] = (byte)(s[0, c] ^ GMul(0x02, s[1, c]) ^ GMul(0x03, s[2, c]) ^ s[3, c]);
                ss[2, c] = (byte)(s[0, c] ^ s[1, c] ^ GMul(0x02, s[2, c]) ^ GMul(0x03, s[3, c]));
                ss[3, c] = (byte)(GMul(0x03, s[0, c]) ^ s[1, c] ^ s[2, c] ^ GMul(0x02, s[3, c]));
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    s[i, j] = ss[i, j];
                }
            }
        }

        

        public void ShiftRowsMatrix(byte [,] s)
        {
            // первая строка не меняется

            // вторая строка
            var tmp = s[1, 0];
            s[1, 0] = s[1, 1];
            s[1, 1] = s[1, 2];
            s[1, 2] = s[1, 3];
            s[1, 3] = tmp;

            // третья строка
            var tmp1 = s[2, 0];
            var tmp2 = s[2, 1];
            s[2, 0] = s[2, 2];
            s[2, 1] = s[2, 3];
            s[2, 2] = tmp1;
            s[2, 3] = tmp2;

            // четвертая строка

            tmp1 = s[3, 0];
            tmp2 = s[3, 1];
            var tmp3 = s[3, 2];
            s[3, 0] = s[3, 3];
            s[3, 1] = tmp1;
            s[3, 2] = tmp2;
            s[3, 3] = tmp3;
        }

        public void AddRoundKey(byte[,] s, Word w1, Word w2, Word w3, Word w4)
        {
            //Console.WriteLine($"{w1.b1} {w1.b2} {w1.b3} {w1.b4} || {w2.b1} {w2.b2} {w2.b3} {w2.b4} || {w3.b1} {w3.b2} {w3.b3} {w3.b4}|| {w4.b1} {w4.b2} {w4.b3} {w4.b4}");

            s[0, 0] ^= w1.b1;
            s[1, 0] ^= w1.b2;
            s[2, 0] ^= w1.b3;
            s[3, 0] ^= w1.b4;

            s[0, 1] ^= w2.b1;
            s[1, 1] ^= w2.b2;
            s[2, 1] ^= w2.b3;
            s[3, 1] ^= w2.b4;

            s[0, 2] ^= w3.b1;
            s[1, 2] ^= w3.b2;
            s[2, 2] ^= w3.b3;
            s[3, 2] ^= w3.b4;

            s[0, 3] ^= w4.b1;
            s[1, 3] ^= w4.b2;
            s[2, 3] ^= w4.b3;
            s[3, 3] ^= w4.b4;
        }


        public static void generateMatrix(byte[] bytes, byte[,] s)
        {
            int index = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    s[j, i] = bytes[index];
                    index++;
                }
            }
        }

        public void PrintMatrix(byte[,] s)
        {
            Console.WriteLine("===== Matrix =====");
            for (int i = 0; i < 4; i++)
            {
                string str = "";
                for (int j = 0; j < 4; j++)
                {
                    str += $"{s[i, j] } ";
                }
                Console.WriteLine(str);
            }
            Console.WriteLine("-------------------");
        }
    }
}

