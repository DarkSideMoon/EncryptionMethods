using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Polyalphabetic.Library;

namespace Polyalphabetic.Tests
{
    [TestClass]
    public class AdvancedPolyalphabeticTest
    {
        [TestMethod]
        public void TestAdvancedCodingDecoding1()
        {
            // arrange
            string key = "KEY";
            string word = "HELLO WORLD";
            AdvancedPolyalphabetic polyalphabetic = 
                new AdvancedPolyalphabetic(AdvancedPolyalphabetic.LegalCharacters.Alphabetical, key);   

            // act 
            string encodeString = polyalphabetic.Encode(word);
            string decodeString = polyalphabetic.Decode(encodeString);

            // assert
            Assert.AreEqual(decodeString, word);
        }

        [TestMethod]
        public void TestAdvancedCodingDecoding2()
        {
            // arrange
            string key = "hi";
            string word = "hello";
            AdvancedPolyalphabetic polyalphabetic =
                new AdvancedPolyalphabetic(AdvancedPolyalphabetic.LegalCharacters.Alphabetical, key);

            // act 
            string encodeString = polyalphabetic.Encode(word);
            string decodeString = polyalphabetic.Decode(encodeString);

            // assert
            Assert.AreEqual(decodeString.ToLower(), word);
        }

        [TestMethod]
        public void TestAdvancedCodingDecoding3()
        {
            // arrange
            string key = "test";
            string word = "123";
            AdvancedPolyalphabetic polyalphabetic =
                new AdvancedPolyalphabetic(AdvancedPolyalphabetic.LegalCharacters.Alphanumerical, key);

            // act 
            string encodeString = polyalphabetic.Encode(word);
            string decodeString = polyalphabetic.Decode(encodeString);

            // assert
            Assert.AreEqual(decodeString.ToLower(), word);
        }

        [TestMethod]
        public void TestAdvancedCodingDecoding4()
        {
            // arrange
            string key = "KEY";
            string word = "123";
            AdvancedPolyalphabetic polyalphabetic =
                new AdvancedPolyalphabetic(AdvancedPolyalphabetic.LegalCharacters.Alphanumerical, key);

            // act 
            string encodeString = polyalphabetic.Encode(word);
            string decodeString = polyalphabetic.Decode(encodeString);

            // assert
            Assert.AreEqual(decodeString.ToLower(), word);
        }

        [TestMethod]
        public void TestAdvancedCodingDecoding5()
        {
            // arrange
            string key = "apple";
            string word = "howareyou";
            AdvancedPolyalphabetic polyalphabetic =
                new AdvancedPolyalphabetic(AdvancedPolyalphabetic.LegalCharacters.Alphabetical, key);

            // act 
            string encodeString = polyalphabetic.Encode(word);
            string decodeString = polyalphabetic.Decode(encodeString);

            // assert
            Assert.AreEqual(decodeString.ToLower(), word);
        }
    }
}
