using System;
using System.IO;
using SearchDublicatesScale.Interface;

namespace SearchDublicatesScale.Classes
{
    internal class Dublicates : IDublicates, IRemoveBackUp //IHashFiles,IHashMD5File,ICreateDirectories
    {
        private readonly IHashFiles hashFiles;
        private readonly string rootPath;

       // public IHashFiles HashFiles { get; set; }
       // public string RootPath { get; set; }

        public Dublicates()
        {

        }
        public Dublicates(IHashFiles _hashFiles, string _rootPath)
        {
            hashFiles = _hashFiles;
            rootPath = _rootPath;
        }

        public void MoveFiles(string rootPath)
        {
            hashFiles.GetHashFiles(rootPath);
        }

        public virtual void RemoveBack_Up(string path)
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
                RemoveBack_Up(directory);
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
}
