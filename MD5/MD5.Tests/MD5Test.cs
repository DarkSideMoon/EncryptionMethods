using System;
using System.IO;
using System.Text;
using Externals;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MD5.Library;

namespace MD5.Tests
{
    [TestClass]
    public class MD5Test
    {
        private Library.MD5 _myMD5;

        [TestMethod]
        public void TestMyMd5WithDataFromWebsite()
        {
            // Act 
            this._myMD5 = new Library.MD5();
            string text = "test";

            string expectedHash = "098f6bcd4621d373cade4e832627b4f6".ToUpper();

            _myMD5.Value = text;
            string actualHash = _myMD5.FingerPrint;

            // Assert
            Assert.AreEqual(expectedHash, actualHash);
        }

        [TestMethod]
        public void TestMyMd5WithDataFromWebsite1()
        {
            // Act 
            this._myMD5 = new Library.MD5();
            string text = "pasha";

            string expectedHash = "ec8bc8e2b120d143e7274de2508f3f6f".ToUpper();

            _myMD5.Value = text;
            string actualHash = _myMD5.FingerPrint;

            // Assert
            Assert.AreEqual(expectedHash, actualHash);
        }

        [TestMethod]
        public void TestMyMd5WithDataFromWebsite2()
        {
            // Act 
            this._myMD5 = new Library.MD5();
            string text = "loremipsum";

            string expectedHash = "65a73f29730d3519bd7dd98ab954ed56".ToUpper();

            _myMD5.Value = text;
            string actualHash = _myMD5.FingerPrint;

            // Assert
            Assert.AreEqual(expectedHash, actualHash);
        }

        [TestMethod]
        public void TestMyMd5WithDataFromWebsite3()
        {
            string path = External.GetExternalFolderPath();
            string fileName = "Lorem Ipsum.txt";

            // Act 
            this._myMD5 = new Library.MD5();
            string text = File.ReadAllText(String.Concat(path, fileName));

            string expectedHash = "e71809104f49d2cfdfcab93819543eb9".ToUpper();

            _myMD5.Value = text;
            string actualHash = _myMD5.FingerPrint;

            // Assert
            Assert.AreEqual(expectedHash, actualHash);
        }
    }
}
