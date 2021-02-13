using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DSA.Library
{
    public class SimpleDsa
    {
        private readonly string _hashAlgorithm = "SHA256";
        private RSAParameters publicKey;
        private RSAParameters privateKey;

        public void CreateNewKey()
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(2048))
            {
                rsaProvider.PersistKeyInCsp = false;
                publicKey = rsaProvider.ExportParameters(false);
                privateKey = rsaProvider.ExportParameters(true);
            }
        }

        public byte[] SignData(byte[] hashOfDataToSign)
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(2048))
            {
                rsaProvider.PersistKeyInCsp = false;
                rsaProvider.ImportParameters(privateKey);

                RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsaProvider);
                rsaFormatter.SetHashAlgorithm(_hashAlgorithm);

                return rsaFormatter.CreateSignature(hashOfDataToSign);
            }
        }

        public bool VerifySignature(byte[] hashOfDataToSign, byte[] signature)
        {
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(2048))
            {
                rsaProvider.ImportParameters(publicKey);

                RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsaProvider);
                rsaDeformatter.SetHashAlgorithm(_hashAlgorithm);

                return rsaDeformatter.VerifySignature(hashOfDataToSign, signature);
            }
        }
    }
}
