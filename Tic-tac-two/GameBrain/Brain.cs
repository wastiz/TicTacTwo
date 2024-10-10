namespace GameBrain
{
    public class Brain
    {
        public int[,] board;
        public int[,] movableBoard;
        public int playerNumber = 1;
        public int[] chipsLeft = { 0, 4, 4 };
        public int movableBoardSize;
        public int startRow;
        public int startCol;

        public Brain(int boardSize, int movableBoardSize)
        {
            board = new int[boardSize, boardSize];
            this.movableBoardSize = movableBoardSize;
            
            startRow = (boardSize - movableBoardSize) / 2;
            startCol = (boardSize - movableBoardSize) / 2;
            
            movableBoard = new int[boardSize, boardSize];
            for (int i = 0; i < movableBoardSize; i++)
            {
                for (int j = 0; j < movableBoardSize; j++)
                {
                    movableBoard[startRow + i, startCol + j] = 1;
                }
            }
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
            for (int i = 0; i < movableBoardSize; i++)
            {
                for (int j = 0; j < movableBoardSize; j++)
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
                    if (startRow + movableBoardSize < board.GetLength(0)) startRow++;
                    break;
                case "left":
                    if (startCol - 1 >= 0) startCol--;
                    break;
                case "right":
                    if (startCol + movableBoardSize < board.GetLength(1)) startCol++;
                    break;
                case "up-left":
                    if (startRow - 1 >= 0 && startCol - 1 >= 0)
                    {
                        startRow--;
                        startCol--;
                    }
                    break;
                case "up-right":
                    if (startRow - 1 >= 0 && startCol + movableBoardSize < board.GetLength(1))
                    {
                        startRow--;
                        startCol++;
                    }
                    break;
                case "down-left":
                    if (startRow + movableBoardSize < board.GetLength(0) && startCol - 1 >= 0)
                    {
                        startRow++;
                        startCol--;
                    }
                    break;
                case "down-right":
                    if (startRow + movableBoardSize < board.GetLength(0) && startCol + movableBoardSize < board.GetLength(1))
                    {
                        startRow++;
                        startCol++;
                    }
                    break;
                default:
                    return false;
            }
            
            for (int i = 0; i < movableBoardSize; i++)
            {
                for (int j = 0; j < movableBoardSize; j++)
                {
                    movableBoard[startRow + i, startCol + j] = 1;
                }
            }
            playerNumber = playerNumber == 1 ? 2 : 1;
            return true;
        }
    }
}
