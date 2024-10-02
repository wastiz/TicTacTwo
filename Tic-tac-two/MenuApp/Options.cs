namespace MenuApp
{
    public class Options : Menu
    {
        private MainMenu mainMenu;
        private int grid_size { get; set; } = 5;
        private int movable_grid_size { get; set; } = 3;
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
            else if (size == grid_size)
            {
                HandleGridSize("This size is already assigned...");
            }
            else if (size < 3)
            {
                HandleGridSize("This size is too small...");
            }
            else
            {
                grid_size = size;
                Console.WriteLine($"Grid size successfully updated to {grid_size}.");
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
            else if (size == movable_grid_size)
            {
                HandleMovableGridSize("This size is already assigned...");
            }
            else if (size < 3)
            {
                HandleMovableGridSize("This size is too small...");
            }
            else
            {
                movable_grid_size = size;
                Console.WriteLine($"Movable grid size successfully updated to {movable_grid_size}.");
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
                "Grid Size: " + grid_size, 
                "Movable Grid Size: " + movable_grid_size,
                "Cross Color: " + cross_color,
                "Zero Color: " + zero_color,
                "Back"
            };
            
            actions = new Action[]
            {
                HandleGridSize,          
                HandleMovableGridSize,   
                HandleCrossColor,        
                HandleZeroColor,
                HandleBack
            };

            Start(optionsArray, optionIndex, guidance, actions);
        }
    }
}
