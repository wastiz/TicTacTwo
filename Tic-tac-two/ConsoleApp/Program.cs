namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var mainMenu = new MenuApp.MainMenu();
            var options = new MenuApp.Options();
            var newGameSetup = new MenuApp.NewGameSetup(options);
            
            mainMenu.SetNewGameSetup(newGameSetup);
            mainMenu.SetOptionsSetup(options);
            newGameSetup.SetMainMenu(mainMenu);
            options.SetMainMenu(mainMenu);
            
            
            mainMenu.ShowMainMenu();
        }
    }
}
