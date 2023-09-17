using System;
using System.IO;

namespace CSV_Data_Parser
{
    public class ProgramStarter
    {
        //Main method to be executed in Program.cs Main()
        public void StartProgram()
        {
            ProgramSettings.SetConsoleSettings();
            RunMainMenu();
        }

        //Display main menu of program, show avaliable options to do
        private void RunMainMenu()
        {
            string subTitle = @"
***************************************************************************************************
*   ____  ____ __     __      ____          _                 ____                                *
*  / ___|/ ___|\ \   / /     |  _ \   __ _ | |_  __ _        |  _ \  __ _  _ __  ___   ___  _ __  *
* | |    \___ \ \ \ / /_____ | | | | / _` || __|/ _` | _____ | |_) |/ _` || '__|/ __| / _ \| '__| *
* | |___  ___) | \ V /|_____|| |_| || (_| || |_| (_| ||_____||  __/| (_| || |   \__ \|  __/| |    *
*  \____||____/   \_/        |____/  \__,_| \__|\__,_|       |_|    \__,_||_|   |___/ \___||_|    *
*                                                                                                 *
***************************************************************************************************

Welcome to the CSV-Data-Parser. What would you like to do?
(Use the arrow keys to cycle through options and press enter to select an option.)
";


            string[] options = { "Start", "About", "Exit" };
            Menu mainMenu = new Menu(subTitle, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    BeginWork();
                    break;
                case 1:
                    DisplayAboutInfo();
                    break;
                case 2:
                    ExitProgram();
                    break;
            }
        }

        //kill process on exit
        private void ExitProgram()
        {
            Console.WriteLine("Exiting program. Goodbye.");
            Environment.Exit(0);
        }

        //display information about programm, author etc.
        private void DisplayAboutInfo()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("* * * * * * * * * * * * * * * * * * * * *");
            Console.WriteLine("* Program name: CSV-Data-Parser         *");
            Console.WriteLine("* Version: 1.0                          *");
            Console.WriteLine("* Project type: Console application     *");
            Console.WriteLine("* Framework: .NET Core 3.1              *");
            Console.WriteLine("* Date: December 2022                   *");
            Console.WriteLine("* Author: Artur Lekki                   *");
            Console.WriteLine("* E-mail: a.lekki1990@gmail.com         *");
            Console.WriteLine("* * * * * * * * * * * * * * * * * * * * *");
            Console.ResetColor();

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey(true);
            RunMainMenu();
        }

        private void BeginWork()
        {
            Console.Write("To process the data (matrix to linear), enter file path:\n");
            string fullFilePath = Console.ReadLine();

            if ((!File.Exists(fullFilePath)) || (Path.GetExtension(fullFilePath) != ".csv"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Wrong path/extension or file does not exist.");
                Console.ResetColor();

                BeginWork();
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension(fullFilePath);
                string fileExtension = Path.GetExtension(fullFilePath);
                string fileDirectory = Path.GetDirectoryName(fullFilePath);
                string fileNewPath = Path.Combine(fileDirectory, fileName + "_processed" + fileExtension);

                FileGetCredentials fgc = new FileGetCredentials(fullFilePath, fileName, fileExtension, fileDirectory, fileNewPath);
                bool status = fgc.OpenFile();

                Console.WriteLine(string.Format("Read file. 1/6 = {0}", status ? "OK" : "Error"));
                if (status == false)
                {
                    Console.WriteLine("Press eny key to go back to menu...");
                    Console.ReadKey(true);
                    StartProgram();
                }

                dynamic dynObjFGD = new FileGetData();
                status = dynObjFGD.ValidateData(fgc.FileRowsList);
                Console.WriteLine(string.Format("Validate data. 2/6 = {0}", status ? "OK" : "Error"));
                if (status == false)
                {
                    Console.WriteLine("Press eny key to go back to menu...");
                    Console.ReadKey(true);
                    StartProgram();
                }

                status = dynObjFGD.SetColumns();
                Console.WriteLine(string.Format("Set columns. 3/6 = {0}", status ? "OK" : "Error"));
                if (status == false)
                {
                    Console.WriteLine("Press eny key to go back to menu...");
                    Console.ReadKey(true);
                    StartProgram();
                }


                FileProcessData fpd = new FileProcessData();
                status = fpd.Set2dTable(dynObjFGD.ValidatedRows, dynObjFGD.Dict);
                Console.WriteLine(string.Format("Set Two-dimensional table. 4/6 = {0}", status ? "OK" : "Error"));
                if (status == false)
                {
                    Console.WriteLine("Press eny key to go back to menu...");
                    Console.ReadKey(true);
                    StartProgram();
                }

                status = fpd.SetSectionsConvertData();
                Console.WriteLine(string.Format("Set sections, convert data. 5/6 = {0}", status ? "OK" : "Error"));
                if (status == false)
                {
                    Console.WriteLine("Press eny key to go back to menu...");
                    Console.ReadKey(true);
                    StartProgram();
                }


                FileSave fs = new FileSave();
                status = fs.SaveToFile(fgc, fpd.FinalResults);
                Console.WriteLine(string.Format("Saving data to a file. 6/6 = {0}", status ? "OK" : "Error"));
                if (status == false)
                {
                    Console.WriteLine("Press eny key to go back to menu...");
                    Console.ReadKey(true);
                    StartProgram();
                }


                Console.WriteLine("All done. Press any key to return to the menu...");
                Console.ReadKey(true);
                RunMainMenu();
            }
        }
    }
}
