namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var mainMenu = new MenuApp.MainMenu();
            var newGameSetup = new MenuApp.NewGameSetup();

            // Устанавливаем ссылки на объекты после их создания
            mainMenu.SetNewGameSetup(newGameSetup);
            newGameSetup.SetMainMenu(mainMenu);

            // Запускаем главное меню
            mainMenu.ShowMainMenu();
        }
    }
}
