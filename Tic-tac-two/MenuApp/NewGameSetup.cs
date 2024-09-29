namespace MenuApp
{
    public class NewGameSetup : Menu
    {
        private MainMenu mainMenu;

        public NewGameSetup(MainMenu menu)
        {
            mainMenu = menu;
        }

        private void HandleOneplayer()
        {
            Console.WriteLine("One player");
        }

        private void HandleTwoplayer()
        {
            Console.WriteLine("Two players");
        }

        private string[] optionsArray = new[] { "One player", "Two players", "Back to main menu" };
        private int activeOptionIndex = 0;
        private string menuGuidance = "Press \"Esc\" to exit. Press enter to enter option. Move bt arrows";
        private Action[] menuActions;

        public void ShowNewGameSetup()
        {
            // Обновляем массив действий
            menuActions = new Action[]
            {
                HandleOneplayer,
                HandleTwoplayer,
                mainMenu.ShowMainMenu // Передаем ссылку на метод ShowMainMenu
            };
            Start(optionsArray, activeOptionIndex, menuGuidance, menuActions);
        }
    }
}