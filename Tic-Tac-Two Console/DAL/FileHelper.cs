namespace DAL;

public static class FileHelper
{
    public static string BasePath = Path.Combine(AppContext.BaseDirectory, "tic-tac-two json db") + Path.DirectorySeparatorChar;

    public static string ConfigExtension = ".config.json"; 
    public static string GameExtension = ".game.json";
    public static string SessionExtension = ".session.json";

    static FileHelper()
    {
        if (!Directory.Exists(BasePath))
        {
            Directory.CreateDirectory(BasePath);
        }
    }
}
