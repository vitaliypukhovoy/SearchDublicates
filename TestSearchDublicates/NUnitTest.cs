using System;
using NUnit.Framework;
using SearchDublicates;
using System.IO;
using System.Reflection;
using NUnit.Framework.Internal;

namespace TestSearchDublicates
{
    [TestFixture]
    public class NUnitTest
    {
        static string target = "TestData";
        static string backUp = "BackUp";
        static string CurrentDirectory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).Parent.Parent.FullName;
        string path = Path.Combine(CurrentDirectory, target);
        string targetbackUp = Path.Combine(CurrentDirectory, backUp);
        string currentData = DateTime.Now.ToString("dd/MM/yyyy");

        [Test]
        public void NTestCreateDirectory()
        {
            string directory = "ttttt";
            Dublicates d = new Dublicates(path, currentData);
            object[] args = new object[2];
            args[0] = path;
            args[1] = directory;

            MethodInfo methodInfo = typeof(Dublicates).GetMethod("CreateDirectory", BindingFlags.NonPublic | BindingFlags.Instance);
            string dir = (string)methodInfo.Invoke(d, args);


            Assert.IsNotEmpty(dir);
            Assert.AreEqual(dir, path + "\\" + directory);
        }

        [Test]
        public void NTestGetHashMD5File()
        {
            string textData = Path.Combine("TestData", "1.txt");
            Dublicates d = new Dublicates(path, currentData);

            object[] args = new object[1];
            args[0] = CurrentDirectory + "\\" + textData;
            MethodInfo methodInfo = typeof(Dublicates).GetMethod("GetHashMD5File", BindingFlags.NonPublic | BindingFlags.Instance);
            var md = methodInfo.Invoke(d, args);

            Assert.IsNotNull(md);
        }

        [Test]
        public void NTestExceptionCreateDirectory()
        {
            Dublicates d = new Dublicates(path, currentData);

            object[] args = new object[2];
            args[0] = null;
            args[1] = null;

            MethodInfo methodInfo = typeof(Dublicates).GetMethod("CreateDirectory", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.Catch<TargetInvocationException>(() => methodInfo.Invoke(d, args));
            Assert.Catch<Exception>(() => methodInfo.Invoke(d, args));
        }

        [Test]
        public void NTestGetHashFiles()
        {
            Dublicates d = new Dublicates(targetbackUp, currentData);
            d.GetHashFiles(path);
            Assert.IsNotEmpty(targetbackUp);
        }
    }
}
