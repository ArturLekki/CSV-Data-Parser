using System;
using System.IO;

namespace CSV_Data_Parser
{
    public static class FileLog
    {
        public static void Log(Exception Exc)
        {
            string dateTime = DateTime.Now.ToString();
            string[] dateSplitTime = dateTime.Split(' ');
            string message = Exc.Message.ToString();
            string stackTrace = Exc.StackTrace.ToString();
            string[] stackTraceSplit = stackTrace.Split('\n');
            string where = "";

            for (int i = 0; i < stackTraceSplit.Length; i++)
            {
                if (stackTraceSplit[i].Contains("line") && stackTraceSplit[i].Contains("in "))
                {
                    string trimmed = stackTraceSplit[i].Trim();
                    int whatIndex = trimmed.IndexOf("in ");
                    string trimmedAndCutted = trimmed.Remove(0, whatIndex);

                    where += trimmedAndCutted + "\n";
                }
            }

            string exceptionString = '\n' + "DATE: " + dateSplitTime[0] + " | " + "TIME: " + dateSplitTime[1] + " | " + "MESSAGE: " + message + " | " + "\n" + where;

            string directory = Path.GetFullPath(Environment.CurrentDirectory);
            string path = Path.Combine(directory, "CSV-data-parser-ErrorLog.txt");

            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("*******************************************************");
            Console.WriteLine("* Exception found.\n* Saving information into:\n* " + path);
            File.AppendAllText(path, exceptionString);
            Console.WriteLine("* If you see this message please contact developer at:\n* a.lekki1990@gmail.com and send file created above.");
            Console.WriteLine("*******************************************************");

            Console.ForegroundColor = color;
        }
    }
}
