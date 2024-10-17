namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] board = new int[7, 10];
            // {
            //     { 1, 0, 2, 0, 1, 0, 2 },
            //     { 0, 1, 0, 2, 0, 1, 0 },
            //     { 2, 0, 1, 0, 2, 0, 1 },
            //     { 0, 2, 0, 1, 0, 2, 0 },
            //     { 1, 0, 2, 0, 1, 0, 2 },
            //     { 0, 1, 0, 2, 0, 1, 0 },
            //     { 2, 0, 1, 0, 2, 0, 1 }, 
            //     { 1, 2, 0, 1, 2, 0, 1 }, 
            //     { 0, 1, 2, 0, 1, 2, 0 }, 
            //     { 1, 0, 1, 2, 0, 1, 2 }  
            // };


            int[,] movableBoard = new int[3, 3];

            DrawBoard(board, movableBoard);
        }
        
        static void DrawBoard(int[,] board, int[,] movableBoard)
        {
            int boardHeight = board.GetLength(1);
            int boardWidth = board.GetLength(0);
            int movableBoardHeight = movableBoard.GetLength(1);
            int movableBoardWidth = movableBoard.GetLength(0);
            
            int gridX = 0;
            int gridY = 0;

            for (var row = 0; row < boardWidth; row++)
            {
                for (var col = 0; col < boardHeight; col++)
                {
                    if (board[row, col] == 1)
                        Console.Write(" X ");
                    else if (board[row, col] == 2)
                        Console.Write(" O ");
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
        }


    }
}
