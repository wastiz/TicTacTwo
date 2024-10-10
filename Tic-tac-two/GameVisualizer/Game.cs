using GameBrain;

namespace GameVisualizer
{
    public class Game
    {
        private Brain gameBrain;
        private int[] cursorPosition;
        private bool player1MadeChoice = false;
        private bool player2MadeChoice = false;
        private string optionalMessage = "";

        public Game(int gridSize, int movableGridSize)
        {
            cursorPosition = new int[] { 2, 2 };
            gameBrain = new Brain(gridSize, movableGridSize);
        }

        public void StartGame()
        {
            while (true)
            {
                string playerChip = gameBrain.playerNumber == 1 ? "X" : "O";
                DisplayGrid(gameBrain.board, gameBrain.movableBoard, "Player " + gameBrain.playerNumber + " is making choice (" + playerChip + ")...", optionalMessage);
                if (gameBrain.chipsLeft[gameBrain.playerNumber] <= 0)
                {
                    Console.WriteLine("Game over! No more chips left.");
                    break;
                }
            }
        }

        public void DisplayGrid(int[,] gameBoard, int[,] movableGameBoard, string inputMessage, string optionalMessage = "")
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
                    if (movableGameBoard[i, j] == 1)
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
            ShowPlayerOptions();
        }

        private void ShowPlayerOptions()
        {
            if (gameBrain.chipsLeft[gameBrain.playerNumber] == 2 && !((gameBrain.playerNumber == 1 && player1MadeChoice) || (gameBrain.playerNumber == 2 && player2MadeChoice)))
            {
                Console.WriteLine("Place chip (1)");
                Console.WriteLine("Move Board (2)");
                Console.WriteLine("Remove player's chip (3)");
                
                var pressedKey = Input("Player " + gameBrain.playerNumber + ", make a choice:");
                HandleChoice(pressedKey);
                
                if (gameBrain.playerNumber == 1) 
                    player1MadeChoice = true;
                else 
                    player2MadeChoice = true;
            }
            else
            {
                var pressedKey = Input("Player " + gameBrain.playerNumber + " is thinking:");
                HandleCursor(pressedKey);
            }
        }


        public ConsoleKeyInfo Input(string prompt)
        {
            Console.WriteLine(prompt);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            Console.WriteLine();
            return keyInfo;
        }
        
        public string TextInput(string prompt)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine();
            Console.WriteLine();
            return input;
        }

        private void HandleCursor(ConsoleKeyInfo pressedKey)
        {
            switch (pressedKey.Key)
            {
                case ConsoleKey.UpArrow:
                    HandleArrowMovement(-1, 0, "You cannot move up...");
                    break;
                case ConsoleKey.DownArrow:
                    HandleArrowMovement(1, 0, "You cannot move down...");
                    break;
                case ConsoleKey.LeftArrow:
                    HandleArrowMovement(0, -1, "You cannot move left...");
                    break;
                case ConsoleKey.RightArrow:
                    HandleArrowMovement(0, 1, "You cannot move right...");
                    break;
                case ConsoleKey.Enter:
                    bool madeMove = gameBrain.placeChip(cursorPosition[0], cursorPosition[1]);
                    if (!madeMove)
                    {
                        optionalMessage = "You cannot place here...";
                    }
                    break;
            }
        }

        private void HandleArrowMovement(int deltaX, int deltaY, string errorMessage)
        {
            int startRow = gameBrain.startRow;
            int startCol = gameBrain.startCol;
            
            int newRow = cursorPosition[0] + deltaX;
            int newCol = cursorPosition[1] + deltaY;
            
            if (newRow < startRow || newRow >= startRow + gameBrain.movableBoardSize ||
                newCol < startCol || newCol >= startCol + gameBrain.movableBoardSize)
            {
                optionalMessage = errorMessage;
            }
            else
            {
                cursorPosition[0] = newRow;
                cursorPosition[1] = newCol;
            }
        }


        private void HandleChoice(ConsoleKeyInfo pressedKey)
        {
            switch (pressedKey.Key)
            {
                case ConsoleKey.D1:
                    var nextPressedKey = Input("Player " + gameBrain.playerNumber + " is thinking:");
                    HandleCursor(nextPressedKey);
                    break;

                case ConsoleKey.D2:
                    HandleBoardMove();
                    break;

                case ConsoleKey.D3:
                    var nexttPressedKey = Input("Player " + gameBrain.playerNumber + " is thinking:");
                    HandleCursor(nexttPressedKey);
                    break;

                default:
                    HandleChoice(Input("Invalid choice. Please press 1, 2, or 3."));
                    break;
            }
        }

        private void HandleBoardMove()
        {
            string nextMoveText = TextInput("Please type one of these options: 'up', 'down', 'left', 'right', 'up-left', 'up-right', 'down-left', 'down-right'").ToLower();
            string[] validDirections = { "up", "down", "left", "right", "up-left", "up-right", "down-left", "down-right" };

            if (Array.Exists(validDirections, direction => direction == nextMoveText))
            {
                bool movedBoard = gameBrain.moveMovableBoard(nextMoveText);
                if (movedBoard)
                {
                    Console.WriteLine("Board moved successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to move the board.");
                }
            }
            else
            {
                Console.WriteLine("Invalid direction. Please try again.");
            }
        }
    }
}
