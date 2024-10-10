namespace GameBrain
{
    public class Brain
    {
        public int[,] board;
        public int[,] movableBoard;
        public int playerNumber = 1;
        public int[] chipsLeft = { 0, 4, 4 };
        private int movableBoardSize;
        private int startRow;
        private int startCol;

        public Brain(int boardSize, int movableBoardSize)
        {
            board = new int[boardSize, boardSize];
            this.movableBoardSize = movableBoardSize;

            // Начальная позиция подвижного поля (по центру)
            startRow = (boardSize - movableBoardSize) / 2;
            startCol = (boardSize - movableBoardSize) / 2;

            // Инициализация подвижного поля
            movableBoard = new int[boardSize, boardSize];
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
                chipsLeft[playerNumber]--;
                
                // После 2 фишек можно двигать поле
                if (chipsLeft[playerNumber] == 2)
                {
                    // Сообщение о том, что игрок может двигать поле
                    Console.WriteLine("Now you can move the 3x3 movable board.");
                }

                // Смена игрока
                this.playerNumber = playerNumber == 1 ? 2 : 1;
                return true;
            }
            else
            {
                return false;
            }
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
                    if (startRow - 1 >= 0)
                        startRow--;
                    break;
                case "down":
                    if (startRow + movableBoardSize < board.GetLength(0))
                        startRow++;
                    break;
                case "left":
                    if (startCol - 1 >= 0)
                        startCol--;
                    break;
                case "right":
                    if (startCol + movableBoardSize < board.GetLength(1))
                        startCol++;
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

            return true;
        }

        public void takeOutChip(int x, int y)
        {
            Console.WriteLine("mmm");
        }
    }
}
