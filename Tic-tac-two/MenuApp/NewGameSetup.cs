using GameVisualizer;

namespace MenuApp
{
    public class NewGameSetup : Menu
    {
        private MainMenu mainMenu;
        private Options gameOptions;

        public NewGameSetup(Options options)
        {
            gameOptions = options;
        }

        public void SetMainMenu(MainMenu menu)
        {
            mainMenu = menu;
        }
        
        private void HandleTwoplayer()
        {
            exit = true;
            Game game = new Game(gameOptions._gridSize, gameOptions._movableGridSize);
            game.DisplayGame();
        }
        
        private void HandleOneplayer()
        {
            Console.WriteLine("Player vs AI is not yet realized");
        }

        private void HandleAI()
        {
            Console.WriteLine("AI vs AI is not yet realized");
        }

        private string[] optionsArray = new[] { "Two players", "vs AI", "Back to main menu" };
        private int activeOptionIndex = 0;
        private string menuGuidance = "Press \"Esc\" to exit. Press enter to select an option. Move with arrows";
        private Action[] menuActions;

        public void ShowNewGameSetup()
        {
            menuActions = new Action[]
            {
                HandleTwoplayer,
                HandleOneplayer,
                HandleAI,
                mainMenu.ShowMainMenu
            };

            Start(optionsArray, activeOptionIndex, menuGuidance, menuActions);
        }
    }
}