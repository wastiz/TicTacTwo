namespace GameBrain
{
    public class Brain
    {
        public int[,] board;
        private int[,] movableBoard;
        private int playerNumber = 1;

        public Brain(int board, int movableBoard)
        {
            this.board = new int[board, board];
            this.movableBoard = new int[movableBoard, movableBoard];
        }

        public void makeMove(int x, int y)
        {
            if (board[x, y] == 0)
            {
                movableBoard[x, y] = playerNumber;
                this.playerNumber = playerNumber == 1 ? 2 : 1;
            }
        }
    }
}