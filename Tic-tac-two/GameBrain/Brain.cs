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
        public int[] chipsLeft;
        public int playerNumber = 1;
        private GameConfiguration gameConfig;
        private GameRepositoryDb repository = new GameRepositoryDb();
        public int? win = null;
        
        
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
            chipsLeft = config.ChipsCount;
            gameConfig = config;
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
            chipsLeft = state.ChipsLeft;
            gameConfig = state.GameConfig;
        }

        public void Initialize(GameConfiguration config)
        {
            board = new int[config.BoardSizeHeight, config.BoardSizeWidth];
            boardWidth = config.BoardSizeWidth;
            boardHeight = config.BoardSizeHeight;
            movableBoard = new int[config.MovableBoardHeight, config.MovableBoardWidth];
            movableBoardWidth = config.MovableBoardWidth;
            movableBoardHeight = config.MovableBoardHeight;
            gridX = (board.GetLength(1) - movableBoard.GetLength(1)) / 2;
            gridY = (board.GetLength(0) - movableBoard.GetLength(0)) / 2;
            chipsLeft = config.ChipsCount;
            gameConfig = config;
        }

        public void Initialize(GameState state)
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
            chipsLeft = state.ChipsLeft;
            gameConfig = state.GameConfig;
        }
        
        public void CheckForWinner()
        {
            if (chipsLeft[1] == 0 && chipsLeft[2] == 0)
            {
                win = 0;
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

        public bool placeChip(int x, int y)
        {
            if (board[x, y] == 0)
            {
                board[x, y] = playerNumber;
                CheckForWinner();
                chipsLeft[playerNumber]--;
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
            
            playerNumber = playerNumber == 1 ? 2 : 1;
            return true;
        }

        public void SaveGame(string stateName)
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
                StateName = stateName,
                GameConfig = gameConfig,
                Board = formattedBoard,
                GridX = gridX,
                GridY = gridY,
                ChipsLeft = chipsLeft,
                PlayerNumber = playerNumber
            };
            repository.SaveGameToRepo(gameToSave);
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
