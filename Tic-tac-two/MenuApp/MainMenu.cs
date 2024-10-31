using GameVisualizer;
using DAL;
using GameBrain;

namespace MenuApp
{
    public class MainMenu : Menu
    {
        ConfigRepositoryJson repository = new ConfigRepositoryJson();
        GameRepositoryJson gameRepository = new GameRepositoryJson();
        private string gameMode;
        private string selectedConfigName;
        
        public void ShowMainMenu()
        {
            activeOptionIndex = 0;
            menuGuidance = "Press \"Esc\" to exit. Press enter to select an option. Move with arrows";
            optionsArray = new List<string> { "New Game", "Load Game", "Options", "Exit" };

            menuActions = new Action[]
            {
                ShowNewGameSetup,
                ShowLoadGame,
                ShowOptions,
                HandleExit
            };

            StartMenu();
        }

        public void ShowLoadGame()
        {
            activeOptionIndex = 0;
            menuGuidance = "Choose game configuration or create your own. Press \"Esc\" to exit. Press enter to select an option. Move with arrows";
            optionsArray.Clear();
            List<string> gameStates = gameRepository.GetAllStateNames();

            foreach (var gameState in gameStates)
            {
                optionsArray.Add(gameState);
            }
            optionsArray.Add("Back");
            menuActions = new Action[optionsArray.Count];
            for (int i = 0; i < gameStates.Count; i++)
            {
                int index = i;
                menuActions[index] = () => StartGameWithState(gameStates[index]);
            }
            menuActions[gameStates.Count] = ShowMainMenu;
            
        }

        public void ShowOptions()
        {
            activeOptionIndex = 0;
            menuGuidance = "Choose game configuration or create your own. Press \"Esc\" to exit. Press enter to select an option. Move with arrows";
            
            List<string> allConfigs = repository.GetAllConfigNames();
            
            optionsArray.Clear();
            foreach (string gameConfigName in allConfigs)
            {
                optionsArray.Add(gameConfigName);
            }
    
            optionsArray.Add("Create Game Config");
            optionsArray.Add("Back");
            
            menuActions = new Action[optionsArray.Count];

            for (int i = 0; i < allConfigs.Count; i++)
            {
                int index = i;
                menuActions[index] = () => ShowConfig(allConfigs[index]);
            }
            
            menuActions[allConfigs.Count] = () => ShowCreateConfig();
            menuActions[allConfigs.Count + 1] = ShowMainMenu;

            StartMenu();
        }

        
        public void ShowNewGameSetup()
        {
            optionsArray = new List<string> { "Two players", "PLayer vs AI", "AI vs AI", "Back to main menu" };
            activeOptionIndex = 0;
            menuGuidance = "Press \"Esc\" to exit. Press enter to select an option. Move with arrows";

            menuActions = new Action[]
            {
                HandleTwoplayer,
                HandleOneplayer,
                HandleAI,
                ShowMainMenu
            };

            StartMenu();
        }

        public void ShowConfig(string name)
        {
            optionsArray = new List<string> { "Edit", "Delete", "Back"};
            activeOptionIndex = 0;
            GameConfiguration config = repository.GetConfigurationByName(name);
            menuGuidance = config.ToString();

            menuActions = new Action[]
            {
                ShowEditConfig,
                () => HandleDeleteConfig(config.Name),
                ShowOptions
            };
            StartMenu();
        }

        public void ShowCreateConfig(string optionalMessage = "")
        {
            Console.Clear();
            Console.WriteLine("Creating New Game Configuration");
            Console.WriteLine();
            
            Console.WriteLine(optionalMessage);
            Console.Write("Enter configuration name: ");
            string configName = Console.ReadLine()!.Trim();
            if (string.IsNullOrEmpty(configName))
            {
                ShowCreateConfig("Name cannot be empty");
            }
            
            int boardWidth = RequestIntegerInput("Enter board width (min: 3): ", 3);
            int boardHeight = RequestIntegerInput("Enter board height (min: 3): ", 3);
            
            int movableBoardWidth = RequestIntegerInput("Enter movable board width (min: 3): ", 3);
            int movableBoardHeight = RequestIntegerInput("Enter movable board height (min: 3): ", 3);
            
            int chipsCountX = RequestIntegerInput("Enter number of chips for player X (min: 1): ", 1);
            int chipsCountO = RequestIntegerInput("Enter number of chips for player O (min: 1): ", 1);
            
            int winCondition = RequestIntegerInput("Enter win condition (pieces in a row to win, min: 3): ", 3);
            
            int movePieceAfterNMoves = RequestIntegerInput("Enter number of moves before a piece can be moved (enter 0 to disable): ", 0);
            
            GameConfiguration newConfig = new GameConfiguration
            {
                Name = configName,
                BoardSizeWidth = boardWidth,
                BoardSizeHeight = boardHeight,
                MovableBoardWidth = movableBoardWidth,
                MovableBoardHeight = movableBoardHeight,
                ChipsCount = new int[] { 0, chipsCountX, chipsCountO },
                WinCondition = winCondition,
                MovePieceAfterNMoves = movePieceAfterNMoves
            };
            
            repository.SaveConfiguration(newConfig);
            Console.WriteLine("Configuration saved successfully. Press any key to return.");
            Console.ReadKey();
            ShowOptions();
        }

        private void ShowEditConfig()
        {
            Console.WriteLine("Editing Configuration");
        }
        
        private int RequestIntegerInput(string prompt, int minValue)
        {
            int result;
            bool validInput = false;

            do
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out result) && result >= minValue)
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine($"Invalid input. Please enter a number greater than or equal to {minValue}.");
                }
            }
            while (!validInput);

            return result;
        }

        private void HandleDeleteConfig(string name)
        {
            repository.DeleteConfiguration(name);
            ShowOptions();
        }

        private void HandleTwoplayer()
        {
            gameMode = "two players";
            ShowConfigToChoose();
        }

        private void HandleOneplayer()
        {
            Console.WriteLine("Player vs AI is not yet realized");
        }

        private void HandleAI()
        {
            Console.WriteLine("AI vs AI is not yet realized");
        }

        private void HandleExit()
        {
            Console.Clear();
            Console.WriteLine("Exiting the game");
            Environment.Exit(0);
        }
        
        private void ShowConfigToChoose()
        {
            activeOptionIndex = 0;
            menuGuidance = "Choose config with arrows and enter";
            optionsArray.Clear();
            List<string> confNames = repository.GetAllConfigNames();
            foreach (var name in confNames)
            {
                optionsArray.Add(name);
            }

            menuActions = new Action[optionsArray.Count];

            for (int i = 0; i < confNames.Count; i++)
            {
                int index = i;
                menuActions[index] = () => StartGameWithConf(confNames[index]);
            }
            
            StartMenu();
        }

        private void StartGameWithState(string stateName)
        {
            exit = true;
            Brain gameBrain = new Brain(gameRepository.GetGameStateByName(stateName));
            Game game = new Game(gameMode, gameBrain);
            game.StartGame();
        }

        private void StartGameWithConf(string configName)
        {
            exit = true;
            Brain gameBrain = new Brain(repository.GetConfigurationByName(configName));
            Game game = new Game(gameMode, gameBrain);
            game.StartGame();
        }
    }

}


