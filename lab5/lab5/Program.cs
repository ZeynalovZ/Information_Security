using System;

namespace lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Выберите операцию: \n 1 - создать подпись \n 2 - проверить подпись");
            var operation = Console.ReadLine();
            var sign = new SignatureWorker();


            if (operation == "1")
            {
                sign.CreateSignatureFile("test.txt", "test_sign.txt");
                Console.WriteLine("Подпись успешно создана.");
            }
            else
            {
                var res = sign.VerifySignature("test.txt", "test_sign.txt");
                Console.WriteLine("Результат проверки подписи: " + res);
            }

        }
    }
}
