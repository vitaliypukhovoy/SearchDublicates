using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SearchDublicatesScale.Interface;

namespace SearchDublicatesScale.Classes
{
    internal class HashFiles : IHashFiles
    {
        private readonly IHashMD5File hashMD5File;
        private readonly ICreateDirectories createDirectories;
        private readonly string targetbackUp;
        private readonly string currentData;
        static Dictionary<string, string> hashable = new Dictionary<string, string>();
        int count = 1;
        public HashFiles(IHashMD5File _hashMD5File, ICreateDirectories _createDirectories,
                         string _targetbackUp, string _currentData)
        {
            hashMD5File = _hashMD5File;
            createDirectories = _createDirectories;
            targetbackUp = _targetbackUp;
            currentData = _currentData;
        }
        public void GetHashFiles(string path)
        {
            string[] files = Directory.GetFiles(path);

            if (Directory.Exists(path))
            {
                foreach (var fs in files)
                {
                    if (hashable.All(i => i.Value.Equals(fs, StringComparison.CurrentCultureIgnoreCase))
                        && hashable.ContainsKey(hashMD5File.GetHashMD5File(fs))
                        || hashable.ContainsKey(hashMD5File.GetHashMD5File(fs))
                        )
                    {
                        string currentDirectory = createDirectories.CreateDirectory(targetbackUp, currentData);
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
                        hashable.Add(hashMD5File.GetHashMD5File(fs), fs);
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
    }
}
