using System.Text.Json;
using System.IO;
using DAL;

namespace MenuApp
{
    public class Options : Menu
    {
        
        private MainMenu mainMenu;
        public int _gridSize { get; set; } = 5;
        public int _movableGridSize { get; set; } = 3;
        private string cross_color { get; set; } = "red";
        private string zero_color { get; set; } = "blue";

        public void SetMainMenu(MainMenu menu)
        {
            mainMenu = menu;
        }
        
        private void HandleGridSize()
        {
            HandleGridSize("Enter new grid size:");
        }
        
        private void HandleGridSize(string message)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();
            int size;

            if (!int.TryParse(input, out size))
            {
                HandleGridSize("Invalid input. Please enter a valid number.");
            }
            else if (size == _gridSize)
            {
                HandleGridSize("This size is already assigned...");
            }
            else if (size < 3)
            {
                HandleGridSize("This size is too small...");
            }
            else
            {
                _gridSize = size;
                Console.WriteLine($"Grid size successfully updated to {_gridSize}.");
            }
        }

        private void HandleMovableGridSize()
        {
            HandleMovableGridSize("Enter new movable grid size:");
        }

        private void HandleMovableGridSize(string message)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();
            int size;

            if (!int.TryParse(input, out size))
            {
                HandleMovableGridSize("Invalid input. Please enter a valid number.");
            }
            else if (size == _movableGridSize)
            {
                HandleMovableGridSize("This size is already assigned...");
            }
            else if (size < 3)
            {
                HandleMovableGridSize("This size is too small...");
            }
            else
            {
                _movableGridSize = size;
                
                Console.WriteLine($"Movable grid size successfully updated to {_movableGridSize}.");
            }
        }

        private void HandleCrossColor()
        {
            HandleCrossColor("Enter new cross color:");
        }

        private void HandleCrossColor(string message)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();

            if (input == cross_color)
            {
                HandleCrossColor("This color is already assigned...");
            }
            else if (string.IsNullOrWhiteSpace(input))
            {
                HandleCrossColor("Invalid input. Please enter a valid color.");
            }
            else
            {
                cross_color = input;
                Console.WriteLine($"Cross color successfully updated to {cross_color}.");
            }
        }

        private void HandleZeroColor()
        {
            HandleZeroColor("Enter new zero color:");
        }

        private void HandleZeroColor(string message)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();

            if (input == zero_color)
            {
                HandleZeroColor("This color is already assigned...");
            }
            else if (string.IsNullOrWhiteSpace(input))
            {
                HandleZeroColor("Invalid input. Please enter a valid color.");
            }
            else
            {
                zero_color = input;
                Console.WriteLine($"Zero color successfully updated to {zero_color}.");
            }
        }

        private void HandleBack()
        {
            mainMenu.ShowMainMenu();
        }
        
        private Action[] actions;
        private int optionIndex = 0;
        private string guidance = "Press \"Esc\" to exit. Press enter to select an option. Move with arrows";

        public void ShowOptions()
        {
            string[] optionsArray = new[] 
            { 
                "Grid Size: " + _gridSize, 
                "Movable Grid Size: " + _movableGridSize,
                "Cross Color: " + cross_color,
                "Zero Color: " + zero_color,
                "Back"
            };
            
            actions = new Action[]
            {
                
                HandleBack
            };

            StartMenu(optionsArray, optionIndex, guidance, actions);
        }
    }
}
