namespace MenuApp
{
    public class NewGameSetup : Menu
    {
        private MainMenu mainMenu;

        public void SetMainMenu(MainMenu menu)
        {
            mainMenu = menu;
        }

        private void HandleOneplayer()
        {
            
        }

        private void HandleTwoplayer()
        {
            Console.WriteLine("Two players");
        }

        private string[] optionsArray = new[] { "One player", "Two players", "Back to main menu" };
        private int activeOptionIndex = 0;
        private string menuGuidance = "Press \"Esc\" to exit. Press enter to select an option. Move with arrows";
        private Action[] menuActions;

        public void ShowNewGameSetup()
        {
            menuActions = new Action[]
            {
                HandleOneplayer,
                HandleTwoplayer,
                mainMenu.ShowMainMenu
            };

            Start(optionsArray, activeOptionIndex, menuGuidance, menuActions);
        }
    }
}