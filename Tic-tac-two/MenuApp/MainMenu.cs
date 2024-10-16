namespace MenuApp
{
   public class MainMenu : Menu
   {
       private NewGameSetup newGameSetup;
       private Options newOptions;

       public void SetNewGameSetup(NewGameSetup setup)
       {
           newGameSetup = setup;
       }

       public void SetOptionsSetup(Options options)
       {
           newOptions = options;
       }

       private void HandleNewGame()
       {
           exit = true;
           newGameSetup.ShowNewGameSetup();
       }
       
       private void HandleOptions()
       {
           exit = true;
           newOptions.ShowOptions();
       }
       
       private void HandleExit()
       {
           Console.Clear();
           Console.WriteLine("Exiting the game");
           Environment.Exit(0);
       }

       private string[] mainMenuOptionsArray = new[] { "New Game", "Options", "Exit" };
       private Action[] mainMenuActions;

       private int mainActiveOptionIndex = 0;
       private string mainMenuGuidance = "Press \"Esc\" to exit. Press enter to select an option. Move with arrows";

       public void ShowMainMenu()
       {
           mainMenuActions = new Action[]
           {
               HandleNewGame,
               HandleOptions,
               HandleExit
           };

           StartMenu(mainMenuOptionsArray, mainActiveOptionIndex, mainMenuGuidance, mainMenuActions);
       }
   }
}