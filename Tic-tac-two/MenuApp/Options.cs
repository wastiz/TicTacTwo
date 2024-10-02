namespace MenuApp
{
    public class Options : Menu
    {
        private MainMenu mainMenu;

        public void SetMainMenu(MainMenu menu)
        {
            mainMenu = menu;
        }

        private void HandleGridSize()
        {
            Console.WriteLine("Grid Size:" + "");
        }
        
        private void HandleOptions()
        {
            Console.WriteLine("Enter the options you want to play");
        }

        private void HandleBack()
        {
            mainMenu.ShowMainMenu();
        }
        private string[] optionsArray = new[] { "New Game", "Options", "Exit" };
        private Action[] actions;

        private int optionIndex = 0;
        private string guidance = "Press \"Esc\" to exit. Press enter to select an option. Move with arrows";

        public void ShowOptions()
        {
            actions = new Action[]
            {
                HandleNewGame,
                HandleOptions,
                HandleBack
            };

            Start(optionsArray, optionIndex, guidance, actions);
        }
    }
}

