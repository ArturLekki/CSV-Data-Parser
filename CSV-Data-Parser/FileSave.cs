using System;
using System.Collections.Generic;
using System.IO;

namespace CSV_Data_Parser
{
    public class FileSave
    {
        public bool SaveToFile(FileGetCredentials fgc, List<string> finalResults)
        {
            bool status = false;
            StreamWriter sw = null;

            try
            {
                do
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("* * * * * * * * * * * * * * * * * * * * * * * * * * * *");
                    Console.WriteLine("* Default location to save processed file:");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("* " + fgc.NewPath);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("* Save file to default location? y/n");
                    Console.WriteLine("* * * * * * * * * * * * * * * * * * * * * * * * * * * *");
                    Console.ResetColor();

                    string directoryChoice = Console.ReadLine();

                    if (directoryChoice == "y")
                    {

                        if (File.Exists(fgc.NewPath))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(string.Format("File exists already in directory {0}", fgc.NewPath));
                            Console.WriteLine("Do you want to OVERRIDE existing file? y/n");
                            Console.ResetColor();
                            string overrideChoice = Console.ReadLine();
                            if (overrideChoice == "y")
                            {
                                sw = new StreamWriter(fgc.NewPath);
                                foreach (var item in finalResults)
                                {
                                    sw.WriteLine(item);
                                }
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("File saved in:\n" + fgc.NewPath);
                                Console.ResetColor();
                                status = true;
                            }
                            else if (overrideChoice == "n")
                            {
                                Console.WriteLine("Press any key to go back...");
                                Console.ReadKey(true);
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("Unrecognized command.");
                            }
                        }
                        else
                        {
                            sw = new StreamWriter(fgc.NewPath);
                            foreach (var item in finalResults)
                            {
                                sw.WriteLine(item);
                            }
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("File saved in:\n" + fgc.NewPath);
                            Console.ResetColor();
                            status = true;
                        }
                    }
                    else if (directoryChoice == "n")
                    {
                        Console.WriteLine("Enter new directory: ");
                        fgc.UsersFileDirectory = Console.ReadLine();
                        Console.WriteLine("Enter new file name (without extension) :");
                        fgc.UsersFileName = Console.ReadLine();
                        Console.WriteLine("Enter new file extension (without .):");
                        fgc.UsersExtension = Console.ReadLine();
                        fgc.UsersNewPath = Path.Combine(fgc.UsersFileDirectory, fgc.UsersFileName + "." + fgc.UsersExtension);

                        if (!Directory.Exists(fgc.UsersFileDirectory))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Unable to find directory: " + fgc.UsersFileDirectory);
                            Console.ResetColor();
                        }
                        else if (File.Exists(fgc.UsersNewPath))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(string.Format("File exists already in directory {0}", fgc.UsersNewPath));
                            Console.ResetColor();
                        }
                        else
                        {
                            sw = new StreamWriter(fgc.UsersNewPath);
                            foreach (var item in finalResults)
                            {
                                sw.WriteLine(item);
                            }
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("File saved in:\n" + fgc.UsersNewPath);
                            Console.ResetColor();
                            status = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unrecognized command.");
                    }
                }
                while (status == false);
            }
            catch (Exception exc)
            {
#if DEBUG
                Console.WriteLine(exc.ToString());
#endif
                FileLog.Log(exc);
                status = false;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
            return status;
        }
    }
}
