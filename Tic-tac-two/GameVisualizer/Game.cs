using GameBrain;
namespace GameVisualizer
{
    public class Game
    {
        private Brain gameBrain;
        private int[,] cursorPosition;
        
        public Game(int gridSize, int movableGridSize)
        {
            cursorPosition = new int[gridSize, gridSize];
            cursorPosition[0, 0] = 3;
            gameBrain = new Brain(gridSize, movableGridSize);
        }

        public void DisplayGame()
        {
            DisplayGrid(gameBrain.board);
        }

        public void DisplayGrid(int[,] gameBoard)
        {
            int rows = gameBoard.GetLength(0);
            int columns = gameBoard.GetLength(1);

            Console.Clear();
            
            Console.WriteLine(" ----------------------");

            for (int i = 0; i < rows; i++)
            {
                Console.Write(" |");

                for (int j = 0; j < columns; j++)
                {
                    if (gameBoard[i, j] == 1)
                        Console.Write(" X |");
                    else if (gameBoard[i, j] == 2)
                        Console.Write(" O |");
                    else if (gameBoard[i, j] == 3)
                        Console.Write(" _ |");
                    else
                        Console.Write("   |");
                }

                Console.WriteLine();
                Console.WriteLine(" ----------------------");
            }
            var pressedKey = Input("Player 1 is making choise (X)...");
            switch (pressedKey.Key)
            {
                case ConsoleKey.UpArrow:
                    
                    break;

                case ConsoleKey.DownArrow:
                    
                    break;

                case ConsoleKey.Enter:
                    
                    break;
            }
        }

        public ConsoleKeyInfo Input(string prompt)
        {
            Console.WriteLine(prompt);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            Console.WriteLine();
            return keyInfo;
        }

    }
}