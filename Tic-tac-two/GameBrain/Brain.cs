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
        public int startRow;
        public int startCol;
        
        public int[] chipsLeft;
        public int playerNumber = 1;

        public Brain(string gameMode, GameConfiguration config)
        {
            board = new int[config.BoardSizeWidth, config.BoardSizeHeight];
            boardWidth = config.BoardSizeWidth;
            boardHeight = config.BoardSizeHeight;
            
            movableBoardWidth = config.MovableBoardWidth;
            movableBoardHeight = config.MovableBoardHeight;
            
            startRow = (config.BoardSizeWidth - movableBoardWidth) / 2;
            startCol = (config.BoardSizeHeight - movableBoardHeight) / 2;
            
            movableBoard = new int[config.BoardSizeWidth, config.BoardSizeHeight];
            for (int i = 0; i < movableBoardWidth; i++)
            {
                for (int j = 0; j < movableBoardHeight; j++)
                {
                    movableBoard[startRow + i, startCol + j] = 1;
                }
            }
            
            chipsLeft = new int[] {0, config.ChipsCount[0], config.ChipsCount[1]};
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
            for (int i = 0; i < movableBoardWidth; i++)
            {
                for (int j = 0; j < movableBoardHeight; j++)
                {
                    movableBoard[startRow + i, startCol + j] = 0;
                }
            }
            
            switch (direction)
            {
                case "up":
                    if (startRow - 1 >= 0) startRow--;
                    break;
                case "down":
                    if (startRow + movableBoardWidth < board.GetLength(0)) startRow++;
                    break;
                case "left":
                    if (startCol - 1 >= 0) startCol--;
                    break;
                case "right":
                    if (startCol + movableBoardHeight < board.GetLength(1)) startCol++;
                    break;
                case "up-left":
                    if (startRow - 1 >= 0 && startCol - 1 >= 0)
                    {
                        startRow--;
                        startCol--;
                    }
                    break;
                case "up-right":
                    if (startRow - 1 >= 0 && startCol + movableBoardHeight < board.GetLength(1))
                    {
                        startRow--;
                        startCol++;
                    }
                    break;
                case "down-left":
                    if (startRow + movableBoardWidth < board.GetLength(0) && startCol - 1 >= 0)
                    {
                        startRow++;
                        startCol--;
                    }
                    break;
                case "down-right":
                    if (startRow + movableBoardWidth < board.GetLength(0) && startCol + movableBoardHeight < board.GetLength(1))
                    {
                        startRow++;
                        startCol++;
                    }
                    break;
                default:
                    return false;
            }
            
            for (int i = 0; i < movableBoardWidth; i++)
            {
                for (int j = 0; j < movableBoardHeight; j++)
                {
                    movableBoard[startRow + i, startCol + j] = 1;
                }
            }
            playerNumber = playerNumber == 1 ? 2 : 1;
            return true;
        }
    }
}
