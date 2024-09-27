namespace MenuApp;

public class NewGameSetup : Menu
{
    private string[] NewGameSetupOptions = new[] { "One player (vs AI)", "Two players", "AI vs AI" };
    
    protected void NewGameSetup()
    {
        Console.Clear();
        Console.WriteLine("Setting up new game");
        Console.WriteLine("");
        Console.WriteLine("Select a game option:");
        Console.WriteLine("");
    }
}