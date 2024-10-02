namespace GridVisualizer;

public class Grid
{
    private int row;
    private int column;
    private int[,] grid;

    public Grid(int row, int column)
    {
        this.row = row;
        this.column = column;
        grid = new int[row, column];
    }

    public void DisplayGrid()
    {
        Console.Clear();
        for (int i = 0; i < row; row++)
        {
            for (int j = 0; j < column; j++)
            {
                Console.Write(" - - - ");
                Console.Write("|     |");
                Console.Write(" - - - ");
            }
            Console.WriteLine(" ");
        }
    }
}