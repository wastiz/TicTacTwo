using GameBrain;
using DAL;

namespace GameVisualizer
{
    public class Game
    {
        private Brain gameBrain;
        private int[] cursorPosition;
        private bool player1MadeChoice = false;
        private bool player2MadeChoice = false;
        private string optionalMessage = "";
        private bool gameRunning = true;

        public Game(string gameMode, Brain gameBrain)
        {
            this.gameBrain = gameBrain;
            cursorPosition = new int[] { gameBrain.boardWidth / 2, gameBrain.boardHeight / 2 };
        }

        public void StartGame()
        {
            while (gameRunning)
            {
                string playerChip = gameBrain.playerNumber == 1 ? "X" : "O";
                DisplayGrid(gameBrain.board, gameBrain.movableBoard, gameBrain.gridX, gameBrain.gridY, "Player " + gameBrain.playerNumber + " is making choice (" + playerChip + ")...", optionalMessage);
                if (gameBrain.chipsLeft[gameBrain.playerNumber] <= 0)
                {
                    Console.WriteLine("Game over! No more chips left.");
                    break;
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
                        Console.Write(" X ");
                    else if (board[row, col] == 2)
                        Console.Write(" O ");
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
            if (keyInfo.Key == ConsoleKey.S)
            {
                gameRunning = false;
                string stateName = TextInput("Name the saving...");
                gameBrain.SaveGame(stateName);
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
