using System;
using System.IO;

namespace FdfParser
{
    class Program
    {
        
        static void Main(string[] args)
        {
            bool success = false;
            Console.WriteLine("This console application converts the comments from a fdf file to a target txt file.");
            while (true)
            {
                Console.WriteLine(@"Please enter the path of the target fdf file (for example, C:\filename.fdf)");
                string fdfFilepath = Console.ReadLine();
                if (!FileChecker.CheckFile(fdfFilepath))
                {
                    Console.WriteLine("The file was not a valid one. Do you want to try again? Enter y to continue or anything else to close the application.");
                    string answer = Console.ReadLine();
                    if (answer.Trim().ToLower() == "y")
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Shutting down...");
                        break;
                    }
                }

                string fileName = Path.GetFileNameWithoutExtension(fdfFilepath);

                string destinationDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\FDFParser\" + fileName + ".txt";

                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\FDFParser"))
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\FDFParser");

                Console.WriteLine();
                Console.WriteLine(@"A file will be created in the following directory: " + destinationDirectory);
                Console.WriteLine("If such file already exists, it will be overwritten.");
                Console.WriteLine("Please press enter to start the conversion.");
                Console.ReadLine();
                Console.WriteLine("Starting the conversion...");

                IFDFParser parser = new FDFParser(fdfFilepath);

                var comments = parser.ParseComments();

                File.WriteAllText(destinationDirectory, comments.ToString());

                success = true;

                break;
            }
            if (success)
            {
                Console.WriteLine("The conversion was successful. Shutting down...");
                Console.ReadLine();
            }
        }
    }
}