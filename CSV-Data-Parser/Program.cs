using System;

namespace CSV_Data_Parser
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ProgramStarter programStarter = new ProgramStarter();
                programStarter.StartProgram();
            }
            catch (Exception exc)
            {
#if DEBUG
                Console.WriteLine(exc.ToString());
#endif
                FileLog.Log(exc);
                Environment.Exit(1);
            }
        }
    }
}
