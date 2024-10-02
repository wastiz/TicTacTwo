namespace MenuApp
{
   public class MainMenu : Menu
   {
       private NewGameSetup newGameSetup;

       public void SetNewGameSetup(NewGameSetup setup)
       {
           newGameSetup = setup;
       }

       private void HandleNewGame()
       {
           newGameSetup.ShowNewGameSetup();
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
       private string mainMenuGuidance = "Press \"Esc\" to exit. Press enter to select an option. Move with arrows";

       public void ShowMainMenu()
       {
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