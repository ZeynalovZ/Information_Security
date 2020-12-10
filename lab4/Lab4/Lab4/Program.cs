using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab4
{
    class Program
    {
        private const string _publicKey = "publicKey.txt";
        private const string _privateKey = "privateKey.txt";
        private const string _encryptFilename = "encrypt.rar";
        private const string _decryptFilename = "decrypt.rar";
        private const string _source = "src.rar";

        static string exp;
        static string dxp;
        static string n;


        static void WriteKeys(long exp, long dxp, long n)
        {
            List<string> lstPublic = new List<string>()
            {
                Convert.ToString(exp),
                Convert.ToString(n)
            };
            File.WriteAllLines(_publicKey, lstPublic);

            List<string> lstPrivate = new List<string>()
            {
                Convert.ToString(dxp),
                Convert.ToString(n)
            };
            File.WriteAllLines(_privateKey, lstPrivate);
        }

        static void ReadKeys()
        {
            var publicKey = File.ReadAllLines(_publicKey);
            exp = publicKey[0];
            n = publicKey[1];

            var privateKey = File.ReadAllLines(_privateKey);
            dxp = privateKey[0];
            //var np = privateKey[1];

            Console.WriteLine($"{exp} + {n} == {dxp} + {n}");
        }

        static void EncryptData()
        {
            ReadKeys();
            /*var data = File.ReadAllText(_source, Encoding.UTF8);

            var dataEncrypted = new RSA().EncryptText(data, (exp, n));
            var dataDecrypted = new RSA().DecryptText(dataEncrypted, (dxp, n));

            File.WriteAllText(_encryptFilename, dataEncrypted, Encoding.UTF8);
            File.WriteAllText(_decryptFilename, dataDecrypted, Encoding.UTF8);*/

            EncryptFile(_source, _encryptFilename, new RSA());
            DecryptFile(_encryptFilename, _decryptFilename, new RSA());
        }

        static void EncryptFile(string src, string dst, RSA rsa)
        {
            FileStream fsSrc = new FileStream(src, FileMode.Open);
            FileStream fsDst = new FileStream(dst, FileMode.Create);

            BinaryWriter binWriter = new BinaryWriter(fsDst);
            int cur;
            while (fsSrc.CanRead)
            {
                cur = fsSrc.ReadByte();
                if (cur == -1)
                    break;
                int res = rsa.EncryptBytes(cur, (exp, n));
                binWriter.Write(res);
            }
            binWriter.Write(-1);
            binWriter.Close();
            fsDst.Close();
            fsSrc.Close();
        }

        static void DecryptFile(string src, string dst, RSA rsa)
        {
            FileStream fsSrc = new FileStream(src, FileMode.Open);
            FileStream fsDst = new FileStream(dst, FileMode.Create);
            BinaryReader binReader = new BinaryReader(fsSrc);

            int cur;
            while (fsSrc.CanRead)
            {
                cur = binReader.ReadInt32();
                if (cur == -1)
                    break;
                int res = rsa.DecryptBytes(cur, (dxp, n));
                fsDst.WriteByte((byte)res);
            }

            binReader.Close();
            fsDst.Close();
            fsSrc.Close();
        }

        static void DecryptData()
        {

        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Choose option: \n\t1) Generate keys;\n\t2) Encrypt data;");
                var choosenOption = Console.ReadLine();
                switch (choosenOption)
                {
                    case "1":
                        var kg = new KeyGenerator();
                        WriteKeys(kg.PublicKey.Item1, kg.PrivateKey.Item1, kg.PublicKey.Item2);
                        ReadKeys();
                        break;
                    case "2":
                        EncryptData();
                        break;
                    case "3":

                        break;
                    default:
                        Console.WriteLine("You didn't choose adviced options");
                        break;
                };
            }
            catch (Exception)
            {
                Console.WriteLine("smth goes wroong");
            }
            
        }
    }
}
