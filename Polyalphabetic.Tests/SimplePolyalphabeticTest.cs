using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Polyalphabetic.Library;

namespace Polyalphabetic.Tests
{
    [TestClass]
    public class SimplePolyalphabeticTest
    {
        private SimplePolyalphabetic polyalphabetic;

        public SimplePolyalphabeticTest()
        {
            polyalphabetic = new SimplePolyalphabetic();
        }


        [TestMethod]
        public void TestCountOfAlphabet()
        {
            Assert.IsTrue(polyalphabetic.AlphabetCount == 38);
        }

        [TestMethod]
        public void TestCodingDecoding1()
        {
            // arrange
            string key = "KEY";
            string word = "HELLO WORLD";

            // act 
            string encodeString = polyalphabetic.Encode(word, key);
            string decodeString = polyalphabetic.Decode(encodeString, key);

            // assert
            Assert.AreEqual(decodeString, word);
        }

        [TestMethod]
        public void TestCodingDecoding2()
        {
            // arrange
            string key = "hi";
            string word = "hello";

            // act 
            string encodeString = polyalphabetic.Encode(word, key);
            string decodeString = polyalphabetic.Decode(encodeString, key);

            // assert
            Assert.AreEqual(decodeString.ToLower(), word);
        }

        [TestMethod]
        public void TestCodingDecoding3()
        {
            // arrange
            string key = "SIMPLE KEY";
            string word = "simple text 123";

            // act 
            string encodeString = polyalphabetic.Encode(word, key);
            string decodeString = polyalphabetic.Decode(encodeString, key);

            // assert
            Assert.AreEqual(decodeString.ToLower(), word);
        }

        [TestMethod]
        public void TestCodingDecoding4()
        {
            // arrange
            string key = "SIMPLE KEY";
            string word = "123";

            // act 
            string encodeString = polyalphabetic.Encode(word, key);
            string decodeString = polyalphabetic.Decode(encodeString, key);

            // assert
            Assert.AreEqual(decodeString.ToLower(), word);
        }

        [TestMethod]
        public void TestCodingDecoding5()
        {
            // arrange
            string key = "key";
            string word = "how are you?";

            // act 
            string encodeString = polyalphabetic.Encode(word, key);
            string decodeString = polyalphabetic.Decode(encodeString, key);

            // assert
            Assert.AreEqual(decodeString.ToLower(), word);
        }
    }
}
