using DAL;
using MenuApp;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MainMenu mainMenu = new MainMenu();
            var factory = new AppDbContextFactory();
            var context = factory.CreateDbContext(args);
            context.Database.EnsureCreated();

            mainMenu.ShowMainMenu();
        }
    }

}
