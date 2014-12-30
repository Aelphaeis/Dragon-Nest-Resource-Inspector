using System;
using System.IO;
using DragonNest.ResourceInspector.Pak;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonNest.ResourceInspector.Testing
{
    [TestClass]
    public class PakFileTests
    {
        const string testFile = @"C:\Nexon\DragonNest\Resource11.pak";
        [TestMethod]
        public void TestMethod1()
        {
            //check if file exists
            if (!File.Exists(testFile))
                Assert.Inconclusive("test file does not exist, cannot test LoadPak.");

            //if file does exist open it in read mode
            using(var fileStream = File.Open(testFile, FileMode.Open, FileAccess.Read,FileShare.ReadWrite))
            {
                PakFile pak = new PakFile();
                pak.LoadPak(fileStream);
            }
        }
    }
}
