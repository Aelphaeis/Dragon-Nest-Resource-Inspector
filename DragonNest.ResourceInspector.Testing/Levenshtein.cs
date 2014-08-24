using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DragonNest.ResourceInspector;
namespace DragonNest.ResourceInspector.Testing
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(2, Levenshtein.Distance("book", "back"));
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.AreEqual(5, Levenshtein.Distance("chicken", "Monkey"));
        }

        [TestMethod]
        public void TestMethod3()
        {
            Assert.AreEqual(5, Levenshtein.Distance("Joseph", "Morain"));
        }

        [TestMethod]
        public void TestMethod4()
        {
            Assert.AreEqual(9, Levenshtein.Distance("StarWars", "Treck Star"));
        }

        [TestMethod]
        public void TestMethod5()
        {
            Assert.AreEqual(0D, Levenshtein.Percentage("abcde", "vwxyz"));
        }

        [TestMethod]
        public void TestMethod6()
        {
            Assert.AreEqual(.4D, Levenshtein.Percentage("abcde", "abxyz"));
        }

        [TestMethod]
        public void TestMethod7()
        {
            Assert.AreEqual(1D, Levenshtein.Percentage("abcde", "abcde"));
        }
    }

}
