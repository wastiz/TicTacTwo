namespace GameBrain;

public class MovableGrid
{
    private int row;
    private int column;
    private int[,] grid;

    public MovableGrid(int row, int column)
    {
        this.row = row;
        this.column = column;
        grid = new int[row, column];
    }
}