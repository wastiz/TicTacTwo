namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var mainMenu = new MenuApp.MainMenu();
            var newGameSetup = new MenuApp.NewGameSetup();
            var options = new MenuApp.Options();

            // Устанавливаем ссылки на объекты после их создания
            mainMenu.SetNewGameSetup(newGameSetup);
            mainMenu.SetOptionsSetup(options);
            newGameSetup.SetMainMenu(mainMenu);
            options.SetMainMenu(mainMenu);
            

            // Запускаем главное меню
            mainMenu.ShowMainMenu();
        }
    }
}
