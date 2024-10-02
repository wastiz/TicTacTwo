namespace GridVisualizer;

public class Game
{
    Grid grid = new Grid(3, 3);
    public static void DisplayGame()
    {
        grid.DisplayGrid();
    }
}