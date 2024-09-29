namespace ConsoleApp
{
    using MenuApp;

    class Program
    {
        private static void Main(string[] args)
        {
            // Создаем экземпляры классов MainMenu и NewGameSetup
            NewGameSetup newGameSetup = new NewGameSetup(null); // Передаем временно null
            MainMenu mainMenu = new MainMenu(newGameSetup); // Передаем экземпляр NewGameSetup в MainMenu
            newGameSetup = new NewGameSetup(mainMenu); // Обновляем экземпляр NewGameSetup с MainMenu

            // Запускаем главное меню
            mainMenu.ShowMainMenu();
        }
    }
}