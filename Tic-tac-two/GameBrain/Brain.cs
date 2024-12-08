using DAL;

namespace GameBrain
{
    public class Brain
    {
        public int[,] board;
        public int boardWidth;
        public int boardHeight;
        public int[,] movableBoard;
        public int movableBoardWidth;
        public int movableBoardHeight;
        public int gridX;
        public int gridY;
        public int[] chipsLeft; //chipsLeft[1] - player 1 chips, chipsLeft[2] - player 2 chips
        public int[] playersMoves;
        public int playerNumber;
        public int chipsToOptions; // How many chips have player to place to get options
        public bool player1Options; // player 1 have more options
        public bool player2Options; // player 2 have more options
        private GameConfiguration gameConfig;
        private GameRepositoryDb repository = new GameRepositoryDb();
        public int win = 0; //0 - nothing, 1 - player 1 won, 2 - player 2 won, 3 - draw
        
        
        public Brain () {}
        public Brain(GameConfiguration config)
        {
            board = new int[config.BoardSizeHeight, config.BoardSizeWidth];
            boardWidth = config.BoardSizeWidth;
            boardHeight = config.BoardSizeHeight;
            movableBoard = new int[config.MovableBoardHeight, config.MovableBoardWidth];
            movableBoardWidth = config.MovableBoardWidth;
            movableBoardHeight = config.MovableBoardHeight;
            gridX = (board.GetLength(1) - movableBoard.GetLength(1)) / 2;
            gridY = (board.GetLength(0) - movableBoard.GetLength(0)) / 2;
            playerNumber = 1;
            playersMoves = [0, 0, 0];
            player1Options = false;
            player2Options = false;
            chipsLeft = config.ChipsCount;
            chipsToOptions = config.OptionsAfterNMoves; 
            gameConfig = config;
            win = 0;
        }
        
        
        
        public Brain(GameState state)
        {
            board = new int[state.GameConfig.BoardSizeHeight, state.GameConfig.BoardSizeWidth];
            boardWidth = state.GameConfig.BoardSizeWidth;
            boardHeight = state.GameConfig.BoardSizeHeight;
            

            for (int i = 0; i < boardHeight; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    board[i, j] = state.Board[i][j];
                }
            }
            
            movableBoard = new int[state.GameConfig.MovableBoardHeight, state.GameConfig.MovableBoardWidth];
            movableBoardWidth = state.GameConfig.MovableBoardWidth;
            movableBoardHeight = state.GameConfig.MovableBoardHeight;
            gridX = state.GridX;
            gridY = state.GridY;
            playerNumber = state.PlayerNumber;
            player1Options = state.Player1Options;
            player2Options = state.Player2Options;
            chipsLeft = state.ChipsLeft;
            playersMoves = state.PlayersMoves;
            chipsToOptions = state.GameConfig.OptionsAfterNMoves;
            gameConfig = state.GameConfig;
            win = state.Win;
        }
        
        public void CheckForWinner()
        {
            if (chipsLeft[1] == 0 && chipsLeft[2] == 0)
            {
                win = 3;
            }
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j <= boardHeight - 3; j++)
                {
                    if (board[i, j] != 0 &&
                        board[i, j] == board[i, j + 1] &&
                        board[i, j] == board[i, j + 2])
                    {
                        win = playerNumber;
                    }
                }
            }
            
            for (int j = 0; j < boardHeight; j++)
            {
                for (int i = 0; i <= boardWidth - 3; i++)
                {
                    if (board[i, j] != 0 &&
                        board[i, j] == board[i + 1, j] &&
                        board[i, j] == board[i + 2, j])
                    {
                        win = playerNumber;
                    }
                }
            }

            for (int i = 0; i <= boardWidth - 3; i++)
            {
                for (int j = 0; j <= boardHeight - 3; j++)
                {
                    if (board[i, j] != 0 &&
                        board[i, j] == board[i + 1, j + 1] &&
                        board[i, j] == board[i + 2, j + 2])
                    {
                        win = playerNumber;
                    }
                }
            }
            
            for (int i = 0; i <= boardWidth - 3; i++)
            {
                for (int j = 2; j < boardHeight; j++)
                {
                    if (board[i, j] != 0 &&
                        board[i, j] == board[i + 1, j - 1] &&
                        board[i, j] == board[i + 2, j - 2])
                    {
                        win = playerNumber;
                    }
                }
            }
        }

        public void CheckForOptions()
        {
            if (playersMoves[1] == chipsToOptions)
            {
                Console.WriteLine(playersMoves[1]);
                Console.WriteLine(chipsToOptions);
                Console.WriteLine("got");
                player1Options = true;
            }

            if (playersMoves[2] == chipsToOptions)
            {
                Console.WriteLine("got2");
                player2Options = true;
            }
        }

        public bool placeChip(int x, int y)
        {
            if (board[x, y] == 0)
            {
                board[x, y] = playerNumber;
                CheckForWinner();
                chipsLeft[playerNumber]--;
                CheckForOptions();
                playersMoves[playerNumber] += 1;
                playerNumber = playerNumber == 1 ? 2 : 1;
                return true;
            }
            return false;
        }

        public bool moveChip(int sourceX, int sourceY, int targetX, int targetY)
        {
            if (board[sourceX, sourceY] == playerNumber && board[targetX, targetY] == 0)
            {
                board[targetX, targetY] = playerNumber;
                board[sourceX, sourceY] = 0;
                playersMoves[playerNumber] += 1;
                playerNumber = playerNumber == 1 ? 2 : 1;
                return true;
            }
            return false;
        }
        
        public bool takeOutChip(int x, int y)
        {
            if (board[x, y] != 0)
            {
                board[x, y] = 0;
                CheckForWinner();
                chipsLeft[board[x, y]]++;
                playersMoves[playerNumber] += 1;
                playerNumber = playerNumber == 1 ? 2 : 1;
                return true;
            }

            return false;
        }

        public bool moveMovableBoard(string direction)
        {
            int newGridX = gridX;
            int newGridY = gridY;
            
            switch (direction)
            {
                case "up":
                    if (newGridY - 1 >= 0) newGridY--;
                    break;
                case "down":
                    if (newGridY + movableBoardHeight < boardHeight) newGridY++;
                    break;
                case "left":
                    if (newGridX - 1 >= 0) newGridX--;
                    break;
                case "right":
                    if (newGridX + movableBoardWidth < boardWidth) newGridX++;
                    break;
                case "up-left":
                    if (newGridY - 1 >= 0 && newGridX - 1 >= 0)
                    {
                        newGridY--;
                        newGridX--;
                    }
                    break;
                case "up-right":
                    if (newGridY - 1 >= 0 && newGridX + movableBoardWidth < boardWidth)
                    {
                        newGridY--;
                        newGridX++;
                    }
                    break;
                case "down-left":
                    if (newGridY + movableBoardHeight < boardHeight && newGridX - 1 >= 0)
                    {
                        newGridY++;
                        newGridX--;
                    }
                    break;
                case "down-right":
                    if (newGridY + movableBoardHeight < boardHeight && newGridX + movableBoardWidth < boardWidth)
                    {
                        newGridY++;
                        newGridX++;
                    }
                    break;
                default:
                    return false;
            }
            
            gridX = newGridX;
            gridY = newGridY;
            
            playersMoves[playerNumber] += 1;
            playerNumber = playerNumber == 1 ? 2 : 1;
            
            return true;
        }

        public void SaveGame(string? stateId = null)
        {
            int[][] formattedBoard = new int[board.GetLength(0)][];
            for (int i = 0; i < board.GetLength(0); i++)
            {
                formattedBoard[i] = new int[board.GetLength(1)];
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    formattedBoard[i][j] = board[i, j];
                }
            }
            GameState gameToSave = new GameState()
            {
                Id = stateId ?? Guid.NewGuid().ToString(),
                Name = "Autosave",
                GameConfig = gameConfig,
                Board = formattedBoard,
                GridX = gridX,
                GridY = gridY,
                ChipsLeft = chipsLeft,
                PlayerNumber = playerNumber,
                PlayersMoves = playersMoves,
                Player1Options = player1Options,
                Player2Options = player2Options,
                Win = win
            };
            repository.SaveGameState(gameToSave);
        }
        
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();

            sb.AppendLine("Game Board:");
            for (int i = 0; i < boardHeight; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    sb.Append(board[i, j] + " ");
                }
                sb.AppendLine();
            }

            sb.AppendLine("Movable Board:");
            for (int i = 0; i < movableBoardHeight; i++)
            {
                for (int j = 0; j < movableBoardWidth; j++)
                {
                    sb.Append(movableBoard[i, j] + " ");
                }
                sb.AppendLine();
            }

            sb.AppendLine($"Grid Position: ({gridX}, {gridY})");
            sb.AppendLine($"Chips Left: Player 1 = {chipsLeft[0]}, Player 2 = {chipsLeft[1]}");
            sb.AppendLine($"Current Player: {playerNumber}");
            sb.AppendLine($"Winner: {(win == null ? "None" : win == 0 ? "Draw" : $"Player {win}")}");

            return sb.ToString();
        }
    }

}
