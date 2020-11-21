using System;
using System.IO;
using System.Numerics;

namespace lab3
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Input textfile name");
            var inFileName = Console.ReadLine();
            var dir = Directory.GetCurrentDirectory();
            Console.WriteLine(dir);
            var fileBytes = File.ReadAllBytes(dir + $"\\{inFileName}");
            Console.WriteLine(fileBytes.Length);

            

            Console.WriteLine("Input  res textfile name");
            var resFileName = Console.ReadLine();

            Console.WriteLine("Input encrypt (1) / decrypt(2)");
            var method = Console.ReadLine();

            byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 };

            var algo = new AES(fileBytes, key);

            byte[] resBytes;
            if (method == "1")
            {
                resBytes = algo.EncryptBytes();
            }
            else
            {
                resBytes = algo.DecryptBytes();
            }

            File.WriteAllBytes(resFileName, resBytes);
        }
    }
}
