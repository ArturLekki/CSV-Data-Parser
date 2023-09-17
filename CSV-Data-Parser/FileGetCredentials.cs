using System;
using System.Collections.Generic;
using System.IO;

namespace CSV_Data_Parser
{
    public class FileGetCredentials
    {
        public string FullFilePath;
        public string FileName;
        public string Extension;
        public string FileDirectory;
        public string NewPath;

        public string UsersFileName;
        public string UsersExtension;
        public string UsersFileDirectory;
        public string UsersNewPath;

        public List<string> FileRowsList = new List<string>();

        public FileGetCredentials(string FullFilePath, string FileName, string Extension, string FileDirectory, string NewPath)
        {
            this.FullFilePath = FullFilePath;
            this.FileName = FileName;
            this.Extension = Extension;
            this.FileDirectory = FileDirectory;
            this.NewPath = NewPath;
        }

        //check if file is open by another process
        public bool GetFileAccess(string Path)
        {
            bool accessStatus = true;

            FileStream fs = null;
            FileInfo fi = new FileInfo(Path);
            try
            {
                fs = fi.Open(FileMode.Open);
            }
            catch (IOException)
            {
                accessStatus = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            return accessStatus;
        }

        //Open file to read, split by lines. Add every line to list
        public bool OpenFile()
        {
            bool status = true;

            if (GetFileAccess(this.FullFilePath) == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nFile: " + this.FullFilePath + "\nIs's currently in use by another process.\nMake sure file is closed and unused, then try again.");
                Console.ResetColor();
                Console.WriteLine("Press any key to go back to main menu...");
                Console.ReadKey(true);
                ProgramStarter ps = new ProgramStarter();
                ps.StartProgram();
            }

            StreamReader sr = null;
            sr = new StreamReader(this.FullFilePath);

            try
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    FileRowsList.Add(line);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e.ToString());
#endif
                status = false;
                FileLog.Log(e);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
            return status;
        }
    }
}
