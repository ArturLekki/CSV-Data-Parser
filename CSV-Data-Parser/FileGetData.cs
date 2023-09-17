using System;
using System.Collections.Generic;

namespace CSV_Data_Parser
{
    public class FileGetData
    {
        public string RowNumber = "Row_";
        public string ColNumber = "Col_";
        public List<string> ValidatedRows = new List<string>();
        public Dictionary<string, object> Dict = new Dictionary<string, object>();

        //Split every line from List, validate values, add validated data to list.
        //Create rows, add validated data to dictionary.
        public bool ValidateData(List<string> UnprocessedData)
        {
            bool status = true;
            try
            {
                string validateRow = "";
                for (int i = 0; i < UnprocessedData.Count; i++)
                {
                    string[] rowSplitted = UnprocessedData[i].Split(';');
                    for (int j = 0; j < rowSplitted.Length; j++)
                    {
                        if (rowSplitted[j] == "" || rowSplitted[j] == " " || rowSplitted[j] == null)
                        {
                            rowSplitted[j] = "0";
                        }
                        validateRow += rowSplitted[j] + ";";
                    }
                    ValidatedRows.Add(validateRow);
                    validateRow = "";
                }

                for (int i = 0; i < ValidatedRows.Count; i++)
                {
                    AddProperty(RowNumber + i, ValidatedRows[i]);
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

        //Create columns from validated data list, and them to dictionary.
        //FileGetCredentials fgc
        public bool SetColumns()
        {
            bool status = true;
            try
            {
                // create columns dynamically from validated rows list
                string[] firstRow = ValidatedRows[0].Split(';');
                int countCells = firstRow.Length;
                for (int i = 0; i < countCells; i++)
                {
                    AddProperty(ColNumber + i, null);
                }

                // fill columns with values from validated rows list
                for (int i = 0; i < ValidatedRows.Count; i++)
                {
                    string[] singleRow = ValidatedRows[i].Split(';');
                    for (int j = 0; j < singleRow.Length; j++)
                    {
                        if (Dict.ContainsKey(ColNumber + j))
                        {
                            Dict[ColNumber + j] += singleRow[j] + ";";
                        }
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

        //Add key/value to dictionary
        public void AddProperty(string name, object value)
        {
            Dict[name] = value;
        }
    }
}
