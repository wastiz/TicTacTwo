using GameBrain;

namespace GameVisualizer
{
    public class Game
    {
        private Brain gameBrain;
        private int[] cursorPosition;

        public Game(int gridSize, int movableGridSize)
        {
            cursorPosition = new int[] { 2, 2 };
            gameBrain = new Brain(gridSize, movableGridSize);
        }

        public void DisplayGame()
        {
            DisplayMovableGrid(gameBrain.board, gameBrain.movableBoard, "Player" + gameBrain.playerNumber + "is making choice (X)...");
        }

        public void DisplayMovableGrid(int[,] gameBoard, int[,] movableGameBoard, string inputMessage, string optionalMessage = "")
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
                    
                    if (i >= 1 && i <= 3 && j >= 1 && j <= 3 && movableGameBoard[i, j] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ResetColor();
                    }
            
                    if (gameBoard[i, j] == 1)
                        Console.Write(" X |");
                    else if (gameBoard[i, j] == 2)
                        Console.Write(" O |");
                    else if (i == cursorPosition[0] && j == cursorPosition[1])
                        Console.Write(" _ |");
                    else
                        Console.Write("   |");
                }
                
                Console.WriteLine();
                Console.WriteLine(" ----------------------");
            }

            Console.ResetColor();
            
            Console.WriteLine(optionalMessage);
            
            if (gameBrain.chipsLeft[gameBrain.playerNumber] == 2)
            {
                var pressedKey = Input(inputMessage);
                HandleChoice(pressedKey);
            }
            else
            {
                var pressedKey = Input(inputMessage);
                HandleInput(pressedKey);
            }
        }


        public ConsoleKeyInfo Input(string prompt)
        {
            Console.WriteLine(prompt);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            Console.WriteLine();
            return keyInfo;
        }

        private void HandleInput(ConsoleKeyInfo pressedKey)
        {
            switch (pressedKey.Key)
            {
                case ConsoleKey.UpArrow:
                    handleArrowUp(cursorPosition);
                    break;

                case ConsoleKey.DownArrow:
                    handleArrowDown(cursorPosition);
                    break;

                case ConsoleKey.LeftArrow:
                    handleArrowLeft(cursorPosition);
                    break;

                case ConsoleKey.RightArrow:
                    handleArrowRight(cursorPosition);
                    break;

                case ConsoleKey.Enter:
                    bool madeMove = gameBrain.makeMove(cursorPosition[0], cursorPosition[1]);
                    if (madeMove)
                    {
                        DisplayMovableGrid(gameBrain.board, gameBrain.movableBoard, "Player " + gameBrain.playerNumber + " is making choice (X)...");
                    }
                    else
                    {
                        DisplayMovableGrid(gameBrain.board, gameBrain.movableBoard, "Player " + gameBrain.playerNumber + " is making choice (X)...", "You cannot place here...");
                    }
                    break;
            }
        }

        private void HandleChoice(ConsoleKeyInfo pressedKey)
        {
            Console.WriteLine("Place chip (1)");
            Console.WriteLine("Move Board (2)");
            Console.WriteLine("Remove player's chip (3)");

            switch (pressedKey.Key)
            {
                case ConsoleKey.D1: // Обработка выбора 1 для размещения фишки
                case ConsoleKey.NumPad1:
                    var nextPressedKey = Input("Player is making choice (X)...");
                    HandleInput(nextPressedKey);
                    break;

                case ConsoleKey.D2: // Обработка выбора 2 для перемещения подвижного поля
                case ConsoleKey.NumPad2:
                    var nextMoveText = Input("Please type one of these options: 'up', 'down', 'left', 'right', 'up-left', 'up-right', 'down-left', 'down-right'").ToLower();

                    // Возможные направления перемещения
                    string[] validDirections = { "up", "down", "left", "right", "up-left", "up-right", "down-left", "down-right" };

                    // Проверка, что введено корректное направление
                    if (Array.Exists(validDirections, direction => direction == nextMoveText))
                    {
                        gameBrain.moveMovableBoard(nextMoveText); // Перемещение подвижного поля
                    }
                    else
                    {
                        Console.WriteLine("Invalid direction. Please try again.");
                    }
                    break;

                case ConsoleKey.D3: // Обработка выбора 3 для удаления фишки игрока
                case ConsoleKey.NumPad3:
                    nextPressedKey = Input("Player is making choice (X)..."); // Обновление переменной
                    HandleInput(nextPressedKey); // Передача аргумента в HandleInput
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please press 1, 2, or 3.");
                    break;
            }
        }


        private void handleArrowUp(int[] cursorPosition)
        {
            if (cursorPosition[0] - 1 < 1)
            { 
                DisplayMovableGrid(gameBrain.board, gameBrain.movableBoard, "Player 1 is making choice (X)...", "You cannot move up...");
            }
            else
            {
                cursorPosition[0]--;
                DisplayMovableGrid(gameBrain.board, gameBrain.movableBoard, "Player 1 is making choice (X)...");
            }
        }

        private void handleArrowDown(int[] cursorPosition)
        {
            if (cursorPosition[0] + 1 > 3)
            {
                DisplayMovableGrid(gameBrain.board, gameBrain.movableBoard, "Player 1 is making choice (X)...", "You cannot move down...");
            }
            else
            {
                cursorPosition[0]++;
                DisplayMovableGrid(gameBrain.board, gameBrain.movableBoard, "Player 1 is making choice (X)...");
            }
        }

        private void handleArrowLeft(int[] cursorPosition)
        {
            if (cursorPosition[1] - 1 < 1)
            {
                DisplayMovableGrid(gameBrain.board, gameBrain.movableBoard, "Player 1 is making choice (X)...", "You cannot move left...");
            }
            else
            {
                cursorPosition[1]--;
                DisplayMovableGrid(gameBrain.board, gameBrain.movableBoard, "Player 1 is making choice (X)...");
            }
        }

        private void handleArrowRight(int[] cursorPosition)
        {
            if (cursorPosition[1] + 1 > 3)
            {
                DisplayMovableGrid(gameBrain.board, gameBrain.movableBoard, "Player 1 is making choice (X)...", "You cannot move right...");
            }
            else
            {
                cursorPosition[1]++;
                DisplayMovableGrid(gameBrain.board, gameBrain.movableBoard, "Player 1 is making choice (X)...");
            }
        }
    }
}
