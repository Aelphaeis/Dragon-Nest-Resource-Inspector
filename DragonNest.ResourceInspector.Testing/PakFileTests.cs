using System;
using System.IO;
using System.Collections.Generic;
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

        [TestMethod]
        public void TestMethod2()
        {
            //file names
            string testFile1 = @"C:\Nexon\DragonNest\Resource01.pak";
            string testFile2 = @"C:\Nexon\DragonNest\Resource02.pak";
            string testFile3 = @"C:\Nexon\DragonNest\Resource03.pak";

            //check if file exists
            if (!File.Exists(testFile1))
                Assert.Inconclusive("test file does not exist, cannot test LoadPak.");
            if (!File.Exists(testFile2))
                Assert.Inconclusive("test file does not exist, cannot test LoadPak.");
            if (!File.Exists(testFile3))
                Assert.Inconclusive("test file does not exist, cannot test LoadPak.");


            PakFile pak1 = new PakFile();
            PakFile pak2 = new PakFile();
            PakFile pak3 = new PakFile();


            //if file does exist open it in read mode
            using (var fileStream = File.Open(testFile1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                pak1.LoadPak(fileStream);
            using (var fileStream = File.Open(testFile2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                pak2.LoadPak(fileStream);
            using (var fileStream = File.Open(testFile3, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                pak3.LoadPak(fileStream);

            PakFile.Merge(new List<PakFile> { pak1, pak2, pak3 });

        }
    }
}
