namespace MenuApp
{
    public class MainMenu : Menu
    {
        private NewGameSetup newGameSetup;

        public MainMenu(NewGameSetup newGameSetup)
        {
            this.newGameSetup = newGameSetup;
        }

        // Методы обработки
        private void HandleNewGame()
        {
            newGameSetup.ShowNewGameSetup(); // Переход к настройкам новой игры
        }

        private void HandleExit()
        {
            Console.WriteLine("Exiting the game");
            Environment.Exit(0);
        }

        private void HandleOptions()
        {
            Console.WriteLine("Enter the options you want to play");
        }

        private string[] mainMenuOptionsArray = new[] { "New Game", "Options", "Exit" };
        private Action[] mainMenuActions;

        private int mainActiveOptionIndex = 0;
        private string mainMenuGuidance = "Press \"Esc\" to exit. Press enter to enter option. Move bt arrows";

        public void ShowMainMenu()
        {
            // Обновляем массив действий
            mainMenuActions = new Action[]
            {
                HandleNewGame,
                HandleOptions,
                HandleExit
            };
            Start(mainMenuOptionsArray, mainActiveOptionIndex, mainMenuGuidance, mainMenuActions);
        }
    }
}