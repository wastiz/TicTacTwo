namespace MenuApp;

public class setConfiguration : Menu
{
    private Options options;
    public int _gridSize { get; set; } = 5;
    public int _movableGridSize { get; set; } = 3;
    private string cross_color { get; set; } = "red";
    private string zero_color { get; set; } = "blue";

    public void Options(Options options)
    {
        this.options = options;
    }
    
    
}