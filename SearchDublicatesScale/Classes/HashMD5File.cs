using System;
using System.Security.Cryptography;
using System.IO;
using SearchDublicatesScale.Interface;

namespace SearchDublicatesScale.Classes
{
    internal class HashMD5File : IHashMD5File
    {
        public string GetHashMD5File(string Name)
        {
            var md = new MD5CryptoServiceProvider();
            string hash = String.Empty;
            using (FileStream fs = new FileStream(Name, FileMode.Open))

                foreach (var str in md.ComputeHash(fs))
                {
                    hash += str.ToString("X2").ToLower();
                }
            return hash;
        }
    }
}
