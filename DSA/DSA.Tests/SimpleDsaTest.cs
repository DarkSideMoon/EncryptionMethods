using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DSA.Library;
using Externals;

namespace DSA.Tests
{
    [TestClass]
    public class SimpleDsaTest
    {
        [TestMethod]
        public void TestDsaLoremIpsumText_ReturnTrue()
        {
            var document = Encoding.UTF8.GetBytes("Lorem Ipsum is simply dummy text of the printing and typesetting industry.");
            byte[] hashedDocument;

            using (var sha256 = SHA256.Create())
                hashedDocument = sha256.ComputeHash(document);

            SimpleDsa digitalSignature = new SimpleDsa();
            digitalSignature.CreateNewKey();

            byte[] signature = digitalSignature.SignData(hashedDocument);
            bool verified = digitalSignature.VerifySignature(hashedDocument, signature);

            Assert.IsTrue(verified);
        }

        [TestMethod]
        public void TestDsaLoremIpsumFullText_ReturnTrue()
        {
            string path = External.GetExternalFolderPath();
            string fileName = "Lorem Ipsum.txt";

            var document = Encoding.UTF8.GetBytes(String.Concat(path, fileName));
            byte[] hashedDocument;

            using (var sha256 = SHA256.Create())
                hashedDocument = sha256.ComputeHash(document);

            SimpleDsa digitalSignature = new SimpleDsa();
            digitalSignature.CreateNewKey();

            byte[] signature = digitalSignature.SignData(hashedDocument);
            bool verified = digitalSignature.VerifySignature(hashedDocument, signature);

            Assert.IsTrue(verified);
        }

        [TestMethod]
        public void TestDsaPDF_ReturnTrue()
        {
            string path = External.GetExternalFolderPath();
            string fileName = "LoremIpsum.pdf";

            var document = Encoding.UTF8.GetBytes(String.Concat(path, fileName));
            byte[] hashedDocument;

            using (var sha256 = SHA256.Create())
                hashedDocument = sha256.ComputeHash(document);

            SimpleDsa digitalSignature = new SimpleDsa();
            digitalSignature.CreateNewKey();

            byte[] signature = digitalSignature.SignData(hashedDocument);
            bool verified = digitalSignature.VerifySignature(hashedDocument, signature);

            Assert.IsTrue(verified);
        }
    }
}
