using System;
using System.IO;

namespace FdfParser
{
    class FileChecker
    {      
        public static bool CheckFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("File does not exist.");
                return false;
            }

            DirectoryInfo directory = new DirectoryInfo(fileName);
            if (directory.Extension != ".fdf")
            {
                Console.WriteLine("The file does not have a fdf extension.");
                return false;
            }

            return true;
        }
    }
}
