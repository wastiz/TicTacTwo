using System;

namespace MenuApp
{
    public abstract class Menu
    {
        private string _heading = "Tic Tac Two";
        protected bool exit = false;
        protected string[] optionsArray;
        private int activeOptionIndex;
        private string menuGuidance;
        private Action[] menuActions;
        
        protected void StartMenu(string[] optionsArray, int activeOptionIndex, string menuGuidance, Action[] menuActions)
        {

            while (true)
            {
                Console.Clear();
                DisplayMenuOptions(optionsArray, menuGuidance, activeOptionIndex);
                var pressedKey = Console.ReadKey(true); // true not to display pressed key

                switch (pressedKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        activeOptionIndex = (activeOptionIndex == 0) ? optionsArray.Length - 1 : activeOptionIndex - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        activeOptionIndex = (activeOptionIndex == optionsArray.Length - 1) ? 0 : activeOptionIndex + 1;
                        break;

                    case ConsoleKey.Enter:
                        menuActions[activeOptionIndex]();
                        break;

                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
                
                if(exit) break;
            }
        }

        private void DisplayMenuOptions(string[] optionsArray, string menuGuidance, int activeOptionIndex)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine();
            Console.WriteLine(_heading);
            Console.WriteLine(menuGuidance);
            Console.WriteLine();
            
            for (int i = 0; i < optionsArray.Length; i++)
            {
                if (i == activeOptionIndex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine(optionsArray[i]);
            }
            
            Console.ForegroundColor = ConsoleColor.Black;
        }
    }
}
