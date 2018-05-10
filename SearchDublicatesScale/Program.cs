using System;
using System.IO;
using SearchDublicatesScale.Classes;
using System.Runtime.CompilerServices;

//[assembly: InternalsVisibleTo("TestSearchDublicates")]

namespace SearchDublicatesScale
{
    class Program
    {
        // There is we should to use DI(Autofac, Niject) and do Unit test by Mock for Interface
        const string rootPath = @"C:\TESTDIR";
        const string targetbackUp = @"C:\backUp";
        string currentData = DateTime.Now.ToString("dd/MM/yyyy");

        static void Main(string[] args)
        {
            if (Directory.Exists(targetbackUp))
            {
                //  d.RemoveBack_Up(targetbackUp);
            }
            // we can use IDublicates and IDublicates as references  in order to encapsulation of RemoveBack_Up method.
            Dublicates dublicates = new Dublicates();
            dublicates.MoveFiles(rootPath);
            Console.WriteLine("All  dublicate files have saved");
            Console.ReadKey();
        }
    }
}
