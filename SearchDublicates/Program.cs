using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading;

[assembly: InternalsVisibleTo("TestSearchDublicates")]

namespace SearchDublicates
{
    internal class Dublicates
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
        public void GetHashFiles(string path)
        {
            string[] files = Directory.GetFiles(path);

            if (Directory.Exists(path))
            {
                foreach (var fs in files)
                {
                    string hash = GetHashMD5File(fs);
                    // if (hashable.All(i => i.Value.Equals(fs, StringComparison.CurrentCultureIgnoreCase))
                    //     && hashable.ContainsKey(hash)
                    //     || hashable.ContainsKey(hash))
                    if (hashable.ContainsKey(hash))
                    {
                        string currentDirectory = CreateDirectory(targetbackUp, currentData);
                        string fileName = Path.Combine(currentDirectory, Path.GetFileName(fs));
                        string newFullPath = fileName;
                        try
                        {
                            if (!File.Exists(fileName))
                            {
                                MoveFileAsync(fs, newFullPath).GetAwaiter();
                            }
                            else
                            {
                                AddSameFileToCurrentDir(fs, newFullPath, currentDirectory).GetAwaiter();
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

        private async Task AddSameFileToCurrentDir(string fs, string newFullPath, string currentDirectory)
        {
           await Task.Run(() =>
            {
                while (File.Exists(newFullPath))
                {
                    string fileWithoutExt = Path.GetFileNameWithoutExtension(fs);
                    string fileExt = Path.GetExtension(fs);
                    string newFileName = String.Format("{0}({1})", fileWithoutExt, count++);
                    newFullPath = Path.Combine(currentDirectory, newFileName + fileExt);
                }
            }).ContinueWith(t=> {
                if (MoveFileAsync(fs, newFullPath).IsCompleted)
                {
                    Console.WriteLine("File was moved");
                };
                if (t.IsFaulted) throw t.Exception;
            });           
        }

        private async Task MoveFileAsync(string fileSource, string destFileName)
        {           
            await Task.Factory.StartNew(() =>
             {
                 File.Move(fileSource, destFileName);
             });           
        }


        //private async Task CopyFileAsync(string fileSource, string destFileName)
        //{
        //    var bufferSize = 4096;
        //    var fileOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
        //    using (FileStream source = new FileStream(fileSource, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, fileOptions))
        //    {
        //        using (FileStream destSource = new FileStream(destFileName, FileMode.CreateNew, FileAccess.Write, FileShare.Write, bufferSize, fileOptions))
        //        {                  
        //            await source.CopyToAsync(destSource, bufferSize)
        //                        .ContinueWith(t =>
        //                        {
        //                            if (t.Exception is AggregateException)
        //                            {
        //                                var aux = t.Exception as AggregateException;
        //                                foreach (var err in aux.InnerExceptions)
        //                                {
        //                                     Console.WriteLine("{0}: {1}", ierr.GetType().Name, err.Message);
        //                                }
        //                            }
        //                        }, TaskContinuationOptions.OnlyOnFaulted);
        //        }
        //    }
        //}

        //private async void DeleteAsync(string fileSource)
        //{
        //    var fileOptions = FileOptions.Asynchronous | FileOptions.DeleteOnClose;
        //    using (FileStream source = new FileStream(fileSource, FileMode.Truncate, FileAccess.Write, FileShare.Delete, 4096, fileOptions))
        //    {
        //        await source.FlushAsync()
        //                     .ContinueWith(t =>
        //                     {
        //                         if (t.Exception is AggregateException)
        //                         {
        //                             var aux = t.Exception as AggregateException;
        //                             foreach (var err in aux.InnerExceptions)
        //                             {
        //                                Console.WriteLine("{0}: {1}", err.GetType().Name, err.Message);
        //                             }
        //                         }
        //                     }, TaskContinuationOptions.OnlyOnFaulted);
        //        File.Delete(fileSource);
        //    }
        //}

        private string CreateDirectory(string targetPath, string targetDirectory = null)
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
            }

            return currentDirectory.FullName ?? backUpTarget.FullName;
        }

        private string GetHashMD5File(string Name)
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


        public void RemoveBackUp(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string directory in Directory.GetDirectories(path))
            {
                RemoveBackUp(directory);
            }

            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                Directory.Delete(path, true);
            }

        }


    }
    class Program
    {
        static string rootPath = @"C:\TESTDIR";

        static void Main(string[] args)
        {

            string targetbackUp = @"C:\backUp";
            string currentData = DateTime.Now.ToString("dd/MM/yyyy");
            Dublicates d = new Dublicates(targetbackUp, currentData);

            if (Directory.Exists(targetbackUp))
            {
                // d.RemoveBackUp(targetbackUp);
            }

            d.GetHashFiles(rootPath);
            Console.WriteLine("All  dublicate files have saved");
            Console.ReadKey();
        }


    }
}
