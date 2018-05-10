using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SearchDublicates;
using System.IO;
using System.Reflection;
using System.Diagnostics;

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
            string dir = d.CreateDirectory(path, directory);
            Assert.IsNotEmpty(dir);
            Assert.AreEqual(dir, path +"\\"+directory);
        }

        [Test]
        public void NTestGetHashMD5File()
        {           
            string textData = Path.Combine("TestData", "1.txt");            
            Dublicates d = new Dublicates(path, currentData);
            string md = d.GetHashMD5File(CurrentDirectory + "\\"+ textData);
            Assert.IsNotNull(md);
        }

        [Test]
        public void NTestExceptionCreateDirectory()
        {
            Dublicates d = new Dublicates(path, currentData);           
            Assert.Throws<NullReferenceException>(() => d.CreateDirectory(null, null));
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
