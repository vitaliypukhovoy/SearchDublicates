using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Collections;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestSearchDublicates")]

namespace SearchDublicates
{
    internal   class Dublicates
    {
        private readonly string targetbackUp;
        private readonly string currentData;
        static Dictionary<string, string> hashable = new Dictionary<string, string>();
        int count = 1;
        public Dublicates(string _targetbackUp, string _currentData)
        {
            targetbackUp = _targetbackUp;
            currentData = _currentData;

        }              
        public  void GetHashFiles(string path)
        {
            string[] files = Directory.GetFiles(path);

            if (Directory.Exists(path))
            {
                foreach (var fs in files)
                {
                    if (hashable.All(i => i.Value.Equals(fs, StringComparison.CurrentCultureIgnoreCase))
                        && hashable.ContainsKey(GetHashMD5File(fs))
                        || hashable.ContainsKey(GetHashMD5File(fs))
                        )
                    {
                        string currentDirectory = CreateDirectory(targetbackUp, currentData);
                        string fileName = Path.Combine(currentDirectory, Path.GetFileName(fs));
                        string newFullPath = fileName;
                        try
                        {
                            if (!File.Exists(fileName))
                            {
                                File.Copy(fs, fileName);
                                File.Delete(fs);
                            }
                            else
                            {
                                while (File.Exists(newFullPath))
                                {
                                    string fileWithoutExt = Path.GetFileNameWithoutExtension(fs);
                                    string fileExt = Path.GetExtension(fs);
                                    string newFileName = String.Format("{0}({1})", fileWithoutExt, count++);
                                    newFullPath = Path.Combine(currentDirectory, newFileName + fileExt);
                                }
                                File.Copy(fs, newFullPath);
                                File.Delete(fs);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Sorry the  process failed ", fs);
                            Console.WriteLine("{0}", e.ToString());
                        }
                    }
                    else
                    {
                        hashable.Add(GetHashMD5File(fs), fs);
                    }
                }

                if (Directory.GetDirectories(path) != null)
                {
                    foreach (var d in Directory.GetDirectories(path))
                    {
                        GetHashFiles(d);
                    }
                }
            }
        }

        public string CreateDirectory(string targetPath, string targetDirectory = null)
        {
            DirectoryInfo backUpTarget = null;
            DirectoryInfo currentDirectory = null;
            try
            {
                if (!Directory.Exists(targetPath))
                {
                    backUpTarget = Directory.CreateDirectory(targetPath);
                    backUpTarget.Create();
                }
                if (Directory.Exists(targetPath))
                {
                    currentDirectory = Directory.CreateDirectory(Path.Combine(targetPath, targetDirectory));
                    currentDirectory.Create();
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("Sorry the  process failed, NullReferenceException ");
                Console.WriteLine("{0}", ex.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Sorry the  process failed " + e.ToString());
                Console.WriteLine("{0}", e.ToString());
            }

            return currentDirectory.FullName;
        }

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

        protected virtual void RemoveBackUp()
        {
            Directory.Delete(targetbackUp, true);
        }


    }
    class Program
    {
        static string rootPath = @"C:\TESTDIR";

        static void Main(string[] args)
        {
            //if (Directory.Exists(targetbackUp))
            //{
            //   // RemoveBackUp();
            //}
             string targetbackUp = @"C:\backUp";
             string currentData = DateTime.Now.ToString("dd/MM/yyyy");

            Dublicates d = new Dublicates(targetbackUp, currentData);         
            d.GetHashFiles(rootPath);
            Console.WriteLine("All  dublicate files have saved");
            Console.ReadKey();
        }

       
    }
}
