namespace MenuApp
{
public abstract class Menu
{
    private string _heading = "Tic Tac Two";
    protected bool exit = false;
    protected List<string> optionsArray;
    protected int activeOptionIndex;
    protected string menuGuidance = "Use up and down arrow to select option, enter to enter...";
    protected Action[] menuActions;
    
    protected void StartMenu()
    {
        while (true)
        {
            Console.Clear();
            if (exit) break;
            
            DisplayMenuOptions(optionsArray, menuGuidance, activeOptionIndex);
            
            var pressedKey = Console.ReadKey(true); // true not to display pressed key

            switch (pressedKey.Key)
            {
                case ConsoleKey.UpArrow:
                    activeOptionIndex = (activeOptionIndex == 0) ? optionsArray.Count - 1 : activeOptionIndex - 1;
                    break;

                case ConsoleKey.DownArrow:
                    activeOptionIndex = (activeOptionIndex == optionsArray.Count - 1) ? 0 : activeOptionIndex + 1;
                    break;

                case ConsoleKey.Enter:
                    menuActions[activeOptionIndex]();
                    break;

                case ConsoleKey.Escape:
                    exit = true;
                    break;
            }
        }
    }
    
    private void DisplayMenuOptions(List<string> optionsArray, string menuGuidance, int activeOptionIndex)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
        Console.WriteLine(_heading);
        Console.WriteLine(menuGuidance);
        Console.WriteLine();
        
        for (int i = 0; i < optionsArray.Count; i++)
        {
            if (i == activeOptionIndex)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine(optionsArray[i]);
        }
        
        Console.ForegroundColor = ConsoleColor.White;
    }
}
}