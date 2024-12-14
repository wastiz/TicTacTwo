using DAL;
using GameBrain;

namespace GameVisualizer
{
    public class Game
    {
        private Brain gameBrain;
        private string stateId;
        private int[] cursorPosition;
        private string optionalMessage = "";
        private bool gameRunning = true;
        private int[] startMovingPosition = [-1, -1];
        private int[] endMovingPosition;

        public Game(string gameMode, Brain gameBrain, string stateId = null)
        {
            stateId = stateId;
            this.gameBrain = gameBrain;
            cursorPosition = new int[] { gameBrain.boardWidth / 2, gameBrain.boardHeight / 2 };
        }

        public void StartGame()
        {
            while (gameRunning)
            {
                string playerChip = gameBrain.playerNumber == 1 ? "X" : "O";
                DisplayGrid(gameBrain.board, gameBrain.movableBoard, gameBrain.gridX, gameBrain.gridY, "Player " + gameBrain.playerNumber + " is making choice (" + playerChip + ")...", optionalMessage);

                if (gameBrain.win == 1)
                {
                    gameRunning = false;
                    Console.WriteLine("Game over! Player 1 wins");
                    Console.WriteLine("Back to Main Menu? (press Enter, other key to exit game)");
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine("Need somehow to show main menu");
                    }
                }

                if (gameBrain.win == 2)
                {
                    gameRunning = false;
                    Console.WriteLine("Game over! Player 2 wins");
                    Console.WriteLine("Back to Main Menu? (press Enter, other key to exit game)");
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine("Need somehow to show main menu");
                    }
                }
                
                if (gameBrain.win == 3)
                {
                    gameRunning = false;
                    Console.WriteLine("Game over! Draw");
                    Console.WriteLine("Back to Main Menu? (press Enter, other key to exit game)");
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine("Need somehow to show main menu");
                    }
                }
            }
        }

        public void DisplayGrid(int[,] board, int[,] movableBoard, int gridX, int gridY, string inputMessage, string optionalMessage = "")
        {
            int boardHeight = board.GetLength(1);
            int boardWidth = board.GetLength(0);
            int movableBoardHeight = movableBoard.GetLength(1);
            int movableBoardWidth = movableBoard.GetLength(0);
            
            Console.Clear();

            for (var row = 0; row < boardWidth; row++)
            {
                for (var col = 0; col < boardHeight; col++)
                {
                    if (board[row, col] == 1)
                        if (cursorPosition[0] == row && cursorPosition[1] == col)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write(" X ");
                            Console.ResetColor();
                        } else if (startMovingPosition[0] == row && cursorPosition[0] == col)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(" X ");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(" X ");
                            Console.ResetColor();
                        }
                    else if (board[row, col] == 2)
                        if (cursorPosition[0] == row && cursorPosition[1] == col) 
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write(" O ");
                            Console.ResetColor();
                        } else if (startMovingPosition[0] == row && cursorPosition[0] == col)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(" O ");
                            Console.ResetColor();
                        } else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(" O ");
                            Console.ResetColor();
                        }
                    else if (cursorPosition[0] == row && cursorPosition[1] == col)
                        Console.Write(" _ ");
                    else
                        Console.Write("   ");

                    if (col == boardHeight - 1)
                    {
                        continue;
                    }
                    else if (row >= gridY && row < gridY + movableBoardWidth &&
                             col >= gridX && col + 1 < gridX + movableBoardHeight)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("|");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("|");
                    }
                }

                Console.WriteLine();

                if (row < boardWidth - 1)
                {
                    for (var col = 0; col < boardHeight; col++)
                    {
                        if (row >= gridY && row < gridY + movableBoardWidth - 1 &&
                            col >= gridX && col < gridX + movableBoardHeight)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("---");
                            Console.ResetColor();
                            if (col < boardHeight - 1 && col + 1 < gridX + movableBoardHeight)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("+");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.Write("+");
                            }
                        }
                        else
                        {
                            Console.Write("---");
                            if (col < boardHeight - 1)
                            {
                                Console.Write("+");
                            }
                        }
                    }
                }

                Console.WriteLine();
            }

            Console.ResetColor();
            Console.WriteLine("To save game press s");
            Console.WriteLine();
            Console.WriteLine(optionalMessage);
            ShowPlayerOptions();
        }

        private void ShowPlayerOptions()
        {
            Console.WriteLine("Player 1 chips left: " + gameBrain.chipsLeft[1]);
            Console.WriteLine("Player 2 chips left: " + gameBrain.chipsLeft[2]);
            
            if (gameBrain.player1Options || gameBrain.player2Options)
            {
                Console.WriteLine();
                Console.WriteLine("Player " + gameBrain.playerNumber + ", you have options:");
                Console.WriteLine("Move Board (1)");
                Console.WriteLine("Remove player's chip (2)");
                
                var pressedKey = Input("Press number from brackets: ");
                HandleChoice(pressedKey);
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
            if (keyInfo.Key == ConsoleKey.S)
            {
                gameRunning = false;
                string stateName = TextInput("Name the saving...");
                if (stateId != null)
                {
                    gameBrain.SaveGame(stateId, stateName);
                }
                else
                {
                    gameBrain.SaveGame(null, stateName);
                }
            }
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
            int startRow = gameBrain.gridY;
            int startCol = gameBrain.gridX;
            
            int newRow = cursorPosition[0] + deltaX;
            int newCol = cursorPosition[1] + deltaY;
            
            if (newRow < startRow || newRow >= startRow + gameBrain.movableBoardWidth ||
                newCol < startCol || newCol >= startCol + gameBrain.movableBoardHeight)
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
                    HandleBoardMove();
                    break;

                case ConsoleKey.D2:
                    HandleBoardMove();
                    break;
                
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
                    if (gameBrain.board[cursorPosition[0], cursorPosition[1]] == gameBrain.playerNumber)
                    {
                        if (startMovingPosition[0] == -1 && startMovingPosition[1] == -1)
                        {
                            startMovingPosition = new[] { cursorPosition[0], cursorPosition[1] };
                            optionalMessage = "Choose a new position for the chip (use arrows, press Enter to confirm).";
                        }
                        else
                        {
                            bool moved = HandleMoveChip(cursorPosition[0], cursorPosition[1]);
                            if (!moved)
                            {
                                optionalMessage = "Invalid move. Try again.";
                            }
                        }
                    }
                    else
                    {
                        if (startMovingPosition[0] == -1 && startMovingPosition[1] == -1)
                        {
                            bool madeMove = gameBrain.placeChip(cursorPosition[0], cursorPosition[1]);
                            if (!madeMove)
                            {
                                optionalMessage = "You cannot place here...";
                            }
                        }
                        else
                        {
                            bool moved = HandleMoveChip(cursorPosition[0], cursorPosition[1]);
                            if (!moved)
                            {
                                optionalMessage = "Invalid move. Try again.";
                            }
                        }
                    }
                    break;

                default:
                    HandleChoice(Input("Invalid choice. Please press option number, place chip or move chip"));
                    break;
            }
        }

        private bool HandleMoveChip(int x, int y)
        {
            if (startMovingPosition[0] == -1 && startMovingPosition[1] == -1)
            {
                if (gameBrain.board[x, y] == gameBrain.playerNumber)
                {
                    startMovingPosition = new[] { x, y };
                    optionalMessage = "Choose a new position for the chip (use arrows, press Enter to confirm).";
                    return false;
                }
                else
                {
                    optionalMessage = "This chip does not belong to you!";
                    return false;
                }
            }
            else
            {
                bool madeMove = gameBrain.moveChip(startMovingPosition[0], startMovingPosition[1], cursorPosition[0], cursorPosition[1]);
                if (madeMove)
                {
                    optionalMessage = "Chip moved successfully!";
                    startMovingPosition = new[] { -1, -1 };
                    return true;
                }
                else
                {
                    optionalMessage = "You cannot place the chip here. Choose another position.";
                    return false;
                }
            }
        }

        private void HandleBoardMove()
        {
            string nextMoveText = TextInput("Please type one of these options: 'up', 'down', 'left', 'right', 'up-left', 'up-right', 'down-left', 'down-right'").ToLower();
            string[] validDirections = { "up", "down", "left", "right", "up-left", "up-right", "down-left", "down-right" };

            if (Array.Exists(validDirections, direction => direction == nextMoveText))
            {
                bool movedBoard = gameBrain.moveMovableBoard(nextMoveText);
                switch (nextMoveText)
                {
                    case "up":
                        cursorPosition[0]--;
                        break;
                    case "down":
                        cursorPosition[0]++;
                        break;
                    case "left":
                        cursorPosition[1]--;
                        break;
                    case "right":
                        cursorPosition[1]++;
                        break;
                    case "up-left":
                        cursorPosition[0]--;
                        cursorPosition[1]--;
                        break;
                    case "up-right":
                        cursorPosition[0]--;
                        cursorPosition[1]++;
                        break;
                    case "down-left":
                        cursorPosition[0]++;
                        cursorPosition[1]--;
                        break;
                    case "down-right":
                        cursorPosition[0]++;
                        cursorPosition[1]++;
                        break;
                }
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
