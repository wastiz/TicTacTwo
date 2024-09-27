using System;

namespace MenuApp
{
    public class Menu
    {
        public delegate void DelegateEscape();
        
        protected string heading = "Tic Tac Two";
        private string[] mainMenuOptionsArray = new[] { "New Game", "Options", "Exit" };
        private string[] mainMenuhandlingOptions = new[] { "New Game", "Options", "Exit" };
        private int activeOptionIndex = 0;
        private string menuGuidance = "Press \"Esc\" to exit. Press enter to enter option. Move bt arrows";
        private void handleEscape()
        {
            Console.WriteLine("Exiting the game");
        }

        public void Start(string[] optionsArray, string[] handlingOptions, int activeOptionIndex, string menuGuidance, DelegateEscape del)
        {
            bool exit = false;

            while (!exit)
            {
                DisplayMenuOptions(optionsArray);
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
                        HandleSelection(handlingOptions); //просто вызывать нужный метод по activeOptionIndex из массива делегатов
                        break;

                    case ConsoleKey.Escape:
                        exit = true;
                        del.Invoke();
                        break;
                }
            }
        }

        private void DisplayMenuOptions(string[] optionsArray)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(heading);
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

        private void HandleSelection(string[] handlingOptions)
        {
            for (int i = 0; i < handlingOptions.Length; i++)
            {
                if(activeOptionIndex == 0){}
            }
            switch (activeOptionIndex)
            {
                case 0:
                    
                    break;

                case 1:
                    Console.Clear();
                    Console.WriteLine("Options");
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("Exiting...");
                    Environment.Exit(0);
                    break;
            }

            Console.WriteLine("Press any key to return to main menu...");
            Console.ReadKey(true);
        }
    }
}
