namespace GameBrain;

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
}