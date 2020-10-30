using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Enigma;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input source filename");
            var fileName = Console.ReadLine();
            Console.WriteLine($"inputed filename = {fileName}");
            var bytes = File.ReadAllBytes(fileName);
            var enigma = new Enigma.Enigma();
            var bytesRes = new byte[bytes.Length];
            for(int i = 0; i < bytes.Length; i++)
            {
                //Console.WriteLine(bytes.ElementAt(i));
                var res = enigma.InputValue(bytes.ElementAt(i));
                bytesRes[i] = (byte)res;
            }
            Console.WriteLine("Input destination file");
            fileName = Console.ReadLine();
            Console.WriteLine($"inputed filename = {fileName}");
            File.WriteAllBytes(fileName, bytesRes);
           
        }
    }
}
