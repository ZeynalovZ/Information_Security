using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Linq;

namespace lab5
{
    public class SignatureWorker
    {
        private string HASH_ALG_NAME = "SHA256";
        private string _modulesFilename = "modules";
        private string _exponentFilename = "exponent";
        RSAParameters rsaKeyInfo = new RSAParameters();
        public SignatureWorker()
        {

        }

        public void CreateSignatureFile(string sourceFileName, string signatureFileName)
        {
            var hashAlgorithm = HashAlgorithm.Create(HASH_ALG_NAME);

            RSA rsa = RSA.Create();
            RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            rsaFormatter.SetHashAlgorithm(HASH_ALG_NAME);

            File.WriteAllBytes(_modulesFilename, rsa.ExportParameters(true).Modulus);
            File.WriteAllBytes(_exponentFilename, rsa.ExportParameters(true).Exponent);

            var signFileStream = File.Create(signatureFileName);
            using (var reader = File.OpenRead(sourceFileName))
            using (var writer = new BinaryWriter(signFileStream))
            {
                var hash = hashAlgorithm.ComputeHash(reader);
                var signature = rsaFormatter.CreateSignature(hash);

                writer.Write(signature);

                reader.Close();
                writer.Close();
            }

        }

        public bool VerifySignature(string sourceFileName, string signatureFileName)
        {

            var hashAlgorithm = HashAlgorithm.Create(HASH_ALG_NAME);
            var rsa = RSA.Create();

            rsaKeyInfo.Modulus = File.ReadAllBytes(_modulesFilename);
            rsaKeyInfo.Exponent = File.ReadAllBytes(_exponentFilename);
            rsa.ImportParameters(rsaKeyInfo);

            RSAPKCS1SignatureDeformatter signatureDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            signatureDeformatter.SetHashAlgorithm(HASH_ALG_NAME);

            bool result = false;

            var signatureStream = File.OpenRead(signatureFileName);
            using (var sourceReader = File.OpenRead(sourceFileName))
            using (var signatureReader = new BinaryReader(signatureStream))
            {

                var hash = hashAlgorithm.ComputeHash(sourceReader);
                var signatureLength = (int)(signatureReader.BaseStream.Length -
                    signatureReader.BaseStream.Position);
                var signature = signatureReader.ReadBytes(signatureLength);

                result = signatureDeformatter.VerifySignature(hash, signature);

                sourceReader.Close();
                signatureReader.Close();
            }

            hashAlgorithm.Dispose();
            rsa.Dispose();

            return result;
        }
    }
}
