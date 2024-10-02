using GameBrain;
namespace MenuApp
{
    public class NewGameSetup : Menu
    {
        private MainMenu mainMenu;

        public void SetMainMenu(MainMenu menu)
        {
            mainMenu = menu;
        }
        
        private void HandleTwoplayer()
        {
            gameBrain = new Game
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
                HandleAI,
                mainMenu.ShowMainMenu
            };

            Start(optionsArray, activeOptionIndex, menuGuidance, menuActions);
        }
    }
}