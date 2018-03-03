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
        [TestMethod]
        public void TestCountOfAlphabet()
        {
            SimplePolyalphabetic polyalphabetic = new SimplePolyalphabetic();

            Assert.IsTrue(polyalphabetic.AlphabetCount == 36);
        }
    }
}
