using System;

namespace CSV_Data_Parser
{
    public class Menu
    {
        private string _currentDateTime;
        private int _selectedIndex;
        private string[] _options;
        private string _subTitle;

        public Menu(string SubTitle, string[] Options)
        {
            _subTitle = SubTitle;
            _options = Options;
            _selectedIndex = 0;
            _currentDateTime = DateTime.Now.ToString();
        }

        //display options that user can choose inside main menu
        private void DisplayOptions()
        {
            Console.WriteLine(_subTitle);
            Console.WriteLine(_currentDateTime);
            Console.ResetColor();

            for (int i = 0; i < _options.Length; i++)
            {
                string currentOption = _options[i];
                string prefix;
                if (i == _selectedIndex)
                {
                    prefix = " *   ";
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    prefix = " ";
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine($" {prefix}<< {currentOption} >>");
            }
            Console.ResetColor();
            _currentDateTime = DateTime.Now.ToString();
        }

        //This method keeps console refresh every time user press button
        public int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                DisplayOptions();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                //update _selectedIndex based on arrow keys
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    _selectedIndex--;
                    if (_selectedIndex == -1)
                    {
                        _selectedIndex = _options.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    _selectedIndex++;
                    if (_selectedIndex == _options.Length)
                    {
                        _selectedIndex = 0;
                    }
                }
            }
            while (keyPressed != ConsoleKey.Enter);

            return _selectedIndex;
        }
    }
}
