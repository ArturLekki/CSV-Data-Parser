using System;
using System.Collections.Generic;

namespace CSV_Data_Parser
{
    public class FileProcessData
    {
        public int MinHeight;
        public int MaxHeight;
        public int MinWidth;
        public int MaxWidth;
        public int CheckValueInt;
        public double CheckValueDouble;

        public string[] ColHeight;
        public string[] RowWidth;
        public string[,] Values;
        public List<string> FinalResults = new List<string>();

        //Create Two-dimensional table that contains all the values
        public bool Set2dTable(List<string> ValidatedList, Dictionary<string, object> Dict)
        {
            bool status = true;
            try
            {
                RowWidth = Dict.GetValueOrDefault("Row_0").ToString().Split(";");
                ColHeight = Dict.GetValueOrDefault("Col_0").ToString().Split(";");

                RowWidth[0] = "0";
                ColHeight[0] = "0";

                string colNumber = "Col_";
                int colCounter = 0;

                //Count only columns from dictionary
                for (int i = 0; i < Dict.Count; i++)
                {
                    if (Dict.ContainsKey(colNumber + i)) colCounter++;
                }

                //create 2-Dimensional table that contains all values (prices)
                Values = new string[ValidatedList.Count, colCounter];

                for (int i = 0; i < Values.GetLength(0); i++)
                {
                    for (int j = 0; j < Values.GetLength(1); j++)
                    {
                        //split every row (i) into temporary table
                        string[] tempTab = ValidatedList[i].Split(';');
                        //copy splitted values into main table
                        Values[i, j] = tempTab[j];
                    }
                }
            }
            catch (Exception exc)
            {
#if DEBUG
                Console.WriteLine(exc.ToString());
#endif
                FileLog.Log(exc);
                status = false;
            }
            return status;
        }

        //Create header, convert data, create sections, add validated and prepared data to List
        public bool SetSectionsConvertData()
        {
            bool status = true;
            try
            {
                string header = "Height min. [mm]" + ';' + "Height max. [mm]" + ";" + "Width min. [mm]" + ';' + "Width max. [mm]" + ";" + "Price [PLN]";
                FinalResults.Add(header);

                for (int i = 1; i < Values.GetLength(0); i++)
                {
                    if (int.TryParse(ColHeight[i - 1], out CheckValueInt) == true)
                    {
                        MinHeight = int.Parse(ColHeight[i - 1]);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(string.Format("Unable to convert value: line = {0}, value = {1}. Press any key to go back to menu...", i, ColHeight[i - 1]));
                        Console.ResetColor();
                        Console.ReadKey(true);
                        ProgramStarter ps = new ProgramStarter();
                        ps.StartProgram();
                    }

                    MinHeight = MinHeight + 1;

                    if (int.TryParse(ColHeight[i], out CheckValueInt) == true)
                    {
                        MaxHeight = int.Parse(ColHeight[i]);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(string.Format("Unable to convert value: line = {0}, value = {1}. Press any key to go back to menu...", i, ColHeight[i]));
                        Console.ResetColor();
                        Console.ReadKey(true);
                        ProgramStarter ps = new ProgramStarter();
                        ps.StartProgram();
                    }


                    string minMaxHeight = MinHeight + ";" + MaxHeight + ";";

                    for (int j = 1; j < Values.GetLength(1) - 1; j++)
                    {
                        if (int.TryParse(RowWidth[j - 1], out CheckValueInt) == true)
                        {
                            MinWidth = int.Parse(RowWidth[j - 1]);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(string.Format("Unable to convert value: column = {0}, value = {1}. Press any key to go back to menu...", j, RowWidth[j - 1]));
                            Console.ResetColor();
                            Console.ReadKey(true);
                            ProgramStarter ps = new ProgramStarter();
                            ps.StartProgram();
                        }

                        MinWidth = MinWidth + 1;

                        if (int.TryParse(RowWidth[j], out CheckValueInt) == true)
                        {
                            MaxWidth = int.Parse(RowWidth[j]);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(string.Format("Unable to convert value: column = {0}, value = {1}. Press any key to go back to menu...", j, RowWidth[j]));
                            Console.ResetColor();
                            Console.ReadKey(true);
                            ProgramStarter ps = new ProgramStarter();
                            ps.StartProgram();
                        }


                        string minMaxWidth = MinWidth + ";" + MaxWidth + ";";
                        string prices = "";

                        if (double.TryParse(Values[i, j], out CheckValueDouble) == true)
                        {
                            prices = double.Parse(Values[i, j]) + ";";
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(string.Format("Unable to convert values: [{0},{1}] = {2}. Press any key to go back to menu...", i, j, Values[i, j]));
                            Console.ResetColor();
                            Console.ReadKey(true);
                            ProgramStarter ps = new ProgramStarter();
                            ps.StartProgram();
                        }

                        string result = minMaxHeight + minMaxWidth + prices;
                        FinalResults.Add(result);
                    }

                }
            }
            catch (Exception exc)
            {
#if DEBUG
                Console.WriteLine(exc.ToString());
#endif
                status = false;
                FileLog.Log(exc);
            }
            return status;
        }
    }
}
