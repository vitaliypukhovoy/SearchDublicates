using System;
using SearchDublicatesScale.Interface;
using System.IO;

namespace SearchDublicatesScale.Classes
{
    internal class CreateDirectories : ICreateDirectories
    {
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
            }

            return currentDirectory.FullName ?? backUpTarget.FullName;
        }
    }
}
