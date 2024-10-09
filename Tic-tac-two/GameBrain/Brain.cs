namespace GameBrain
{
    public class Brain
    {
        public int[,] board;
        public int[,] movableBoard;
        public int playerNumber = 1;

        public Brain(int boardSize, int movableBoardSize)
        {
            board = new int[boardSize, boardSize];
            movableBoard = new int[boardSize, boardSize];
            
            int startRow = (boardSize - movableBoardSize) / 2;
            int startCol = (boardSize - movableBoardSize) / 2;
            
            for (int i = 0; i < movableBoardSize; i++)
            {
                for (int j = 0; j < movableBoardSize; j++)
                {
                    movableBoard[startRow + i, startCol + j] = 1;
                }
            }
        }

        public bool makeMove(int x, int y)
        {
            if (board[x, y] == 0)
            {
                board[x, y] = playerNumber;
                this.playerNumber = playerNumber == 1 ? 2 : 1;
                return true;
            } else
            {
                return false;
            }
        }
    }
}