using System;
using System.IO;
using System.Numerics;

namespace lab3
{
    class Program
    {

        static void Main(string[] args)
        {

            var dir = Directory.GetCurrentDirectory();
            Console.WriteLine(dir);
            var fileBytes = File.ReadAllBytes(dir + "\\text.txt");
            Console.WriteLine(fileBytes.Length);

            byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            var algo = new AES(fileBytes, key);

            var resBytes = algo.EncryptBytes();
            /*
                        Console.WriteLine("===========");
                        string str = "";
                        for (int i = 0; i < resBytes.Length; i++)
                        {
                            str += resBytes[i] + " ";
                        }
                        Console.WriteLine(str);*/

            File.WriteAllBytes("res.txt", resBytes);
        }
    }
}
