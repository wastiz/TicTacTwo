namespace MenuApp
{
    public class Menu
    {
        private string activeTab = "New Game";
        public void Start()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("Welcome to the Tic Tac Two Game!");
            Console.WriteLine();
            Console.WriteLine("New Game");
            Console.WriteLine("Options");
            Console.WriteLine("Exit");
            var pressedKey = Console.ReadKey(true); //true not to display pressed key
            switch (pressedKey.Key)
            {
                case ConsoleKey.UpArrow:
                    
                    break;
                case ConsoleKey.DownArrow:
                    
                    break;
                case ConsoleKey.LeftArrow:
                    Console.WriteLine("Вы нажали стрелку влево.");
                    break;
                case ConsoleKey.RightArrow:
                    Console.WriteLine("Вы нажали стрелку вправо.");
                    break;
                case ConsoleKey.Escape:
                    Console.WriteLine("Вы вышли из программы.");
                    return;
                default:
                    Console.WriteLine("Нажата другая клавиша.");
                    break;
            }
        }

        private void DisplayMenu(string highlighted) 
        {
            switch (highlighted)
            {
                case "New Game":
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("Welcome to the Tic Tac Two Game!");
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("New Game");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("Options");
                    Console.WriteLine("Exit");
                    break;
                case "Options":
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("Welcome to the Tic Tac Two Game!");
                    Console.WriteLine();
                    Console.WriteLine("New Game");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("Options");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("Exit");
                    break;
                case "Exit":
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("Welcome to the Tic Tac Two Game!");
                    Console.WriteLine();
                    Console.WriteLine("New Game");
                    Console.WriteLine("Options");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("Exit");
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
            }
        }
    }
}