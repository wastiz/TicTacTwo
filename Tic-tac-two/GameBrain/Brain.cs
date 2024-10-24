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
        private GameRepositoryJson repository = new GameRepositoryJson();

        public Brain(string gameMode, GameConfiguration config)
        {
            board = new int[config.BoardSizeHeight, config.BoardSizeWidth];
            boardWidth = config.BoardSizeWidth;
            boardHeight = config.BoardSizeHeight;
            movableBoard = new int[config.MovableBoardHeight, config.MovableBoardWidth];
            movableBoardWidth = config.MovableBoardWidth;
            movableBoardHeight = config.MovableBoardHeight;
            gridX = (board.GetLength(1) - movableBoard.GetLength(1)) / 2;
            gridY = (board.GetLength(0) - movableBoard.GetLength(0)) / 2;
            chipsLeft = new int[] {0, config.ChipsCount[0], config.ChipsCount[1]};
            gameConfig = config;
        }

        public bool placeChip(int x, int y)
        {
            if (board[x, y] == 0)
            {
                board[x, y] = playerNumber;
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

    }
}
