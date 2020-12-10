using System;
using System.IO;

namespace Lab6
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Введите названия файла для сжатия данных");
            var fileToCompress = Console.ReadLine();

            var lzw = new LZW();
            var testData = File.ReadAllBytes(fileToCompress);

            var compressedData = lzw.Compress(testData);
            var uncompressedData = lzw.UnCompress(compressedData);

            var isEqual = true;
            for (int i = 0; i < testData.Length; i++)
            {
                if (testData[i] != uncompressedData[i])
                {
                    isEqual = false;
                    break;
                }
            }

            if (isEqual)
                Console.WriteLine("res is equal!");
            else
                Console.WriteLine("res is false!!!!!!!!!!");

            File.WriteAllBytes("testCompress.rar", compressedData);
            File.WriteAllBytes("testUncompressed.rar", uncompressedData);




        }
    }
}
