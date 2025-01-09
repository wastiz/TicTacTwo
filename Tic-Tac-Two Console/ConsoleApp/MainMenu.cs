using ConsoleApp;
using DAL;
using DAL.DTO;
using GameBrain;
using Microsoft.EntityFrameworkCore;

namespace MenuApp
{
    public class MainMenu : Menu
    {
        ConfigRepositoryJson configRepository = new ConfigRepositoryJson();
        GameRepositoryJson gameRepository = new GameRepositoryJson();
        SessionRepositoryJson sessionRepositoryJson = new SessionRepositoryJson();
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
            List<SessionNameAndId> sessionObjects = sessionRepositoryJson.GetAllSessionNamesWithStates();

            foreach (var session in sessionObjects)
            {
                optionsArray.Add(session.SessionName);
            }
            optionsArray.Add("Back");
            menuActions = new Action[optionsArray.Count];
            for (int i = 0; i < sessionObjects.Count; i++)
            {
                int index = i;
                menuActions[index] = () => LoadGameWithSession(sessionObjects[index].SessionId);
            }
            menuActions[sessionObjects.Count] = ShowMainMenu;
            
        }

        public void ShowOptions()
        {
            activeOptionIndex = 0;
            menuGuidance = "Choose game configuration or create your own. Press \"Esc\" to exit. Press enter to select an option. Move with arrows";
            
            List<GameConfiguration> allConfigs = configRepository.GetAllConfigs();
            
            optionsArray.Clear();
            foreach (GameConfiguration gameConfig in allConfigs)
            {
                optionsArray.Add(gameConfig.Name);
            }
    
            optionsArray.Add("Create Game Config");
            optionsArray.Add("Back");
            
            menuActions = new Action[optionsArray.Count];

            for (int i = 0; i < allConfigs.Count; i++)
            {
                int index = i;
                menuActions[index] = () => ShowConfig(allConfigs[index].Id);
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

        public void ShowConfig(string configId)
        {
            optionsArray = new List<string> { "Edit", "Delete", "Back"};
            activeOptionIndex = 0;
            GameConfiguration config = configRepository.GetConfigurationById(configId);
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
                OptionsAfterNMoves = movePieceAfterNMoves
            };
            
            configRepository.SaveConfiguration(newConfig);
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
            configRepository.DeleteConfiguration(name);
            ShowOptions();
        }

        private void HandleTwoplayer()
        {
            gameMode = "two-players";
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
            List<string> confNames = configRepository.GetAllConfigNames();
            foreach (var configName in confNames)
            {
                optionsArray.Add(configName);
            }
            optionsArray.Add("Back");
            
            menuActions = new Action[optionsArray.Count];
            for (int i = 0; i < confNames.Count; i++)
            {
                int index = i;
                menuActions[index] = () => StartGameWithConf(confNames[index]);
            }
            menuActions[confNames.Count] = ShowNewGameSetup;
            
            StartMenu();
        }
        
        private void StartGameWithConf(string configName)
        {
            var config = configRepository.GetConfigurationByName(configName);
    
            if (config == null)
            {
                Console.WriteLine("Game Configuration not found");
                return;
            }
            
            GameSession session = sessionRepositoryJson.CreateSession(config);


            Game game = new Game(session);
            game.StartGame();
        }


        public void LoadGameWithSession(string sessionId)
        {
            GameSession gameSession = sessionRepositoryJson.GetSessionById(sessionId);
            if (gameSession != null)
            {
                Game game = new Game(gameSession);
                game.StartGame();
            }
            else
            {
                Console.WriteLine("Game session not found");
            }
        }
    }

}


