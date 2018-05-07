using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Collections;

namespace SearchDublicates
{
    class Program
    {
         static string rootPath = @"C:\TESTDIR";
         static string backUp = @"C:\backUp";

         static Hashtable hashable = new Hashtable();

        static void Main(string[] args)
        {           
            if (Directory.Exists(backUp))
            {
                RemoveBackUp();
            }
            string dir =  Directory.GetParent(rootPath).FullName;
            GitHashFiles(rootPath);
        }

        public static void GitHashFiles(string path)
        {             
            string[] files = Directory.GetFiles(path);
            if (Directory.Exists(path))
            {
                foreach (var fs in files)
                {
                    if (hashable.Contains(GetHashMD5File(fs)))
                    {
                        int k = 0;
                        string backUpTarget = CreateDirectory(backUp);

                        try
                        {
                            if (!File.Exists(backUpTarget + "\\" + Path.GetFileName(fs)))
                            {
                                File.Copy(fs, backUpTarget + "\\" + Path.GetFileName(fs));
                                File.Delete(fs);
                            }
                            else
                            {

                                File.Copy(fs, backUpTarget + "\\" + (k++) + "-" + Path.GetFileName(fs));
                            }
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("Sorry the  process failed ", e.ToString());
                        }
                    }
                    else
                    {
                        hashable.Add(GetHashMD5File(fs),fs);
                    }
                }

                if (Directory.GetDirectories(path) != null)
                {
                    foreach (var d in Directory.GetDirectories(path))
                    {
                        GitHashFiles(d);
                    }
                }                                  
            }           
        }

        static string CreateDirectory (string targetPath )
        {          
           // string targetPath =  Path.Combine(backUp);
            try
            {
                if (!Directory.Exists(targetPath))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(targetPath);
                    dir.Create();
                }
            }
            catch (Exception e)
            {
               Console.WriteLine("Sorry the  process failed ", e.ToString());
            }

            return targetPath;
        }

        static string GetHashMD5File(string Name)
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

        static void RemoveBackUp()
        {
            Directory.Delete(backUp, true);        
        }
    }
}
