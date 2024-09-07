using FlashCards.Data.Daos.Implementations;
using FlashCards.Data.Daos.Interfaces;
using FlashCards.Helpers;
using FlashCards.Menus;
using Microsoft.Extensions.DependencyInjection;

namespace FlashCards;

internal class Program
{
    static void Main(string[] args)
    {
        // Step 1: Create a service collection (DI container)
        var serviceCollection = new ServiceCollection();

        // Step 2: Register your services
        ConfigureServices(serviceCollection);

        // Step 3: Build the service provider
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Step 4: Request the required service and run your application
        var app = serviceProvider.GetService<Program>();

        FlashCardsHelper flashCardsHelper = serviceProvider.GetRequiredService<FlashCardsHelper>();

        try
        {
            flashCardsHelper.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Register the others menus
        services.AddTransient<MainMenu>();
        services.AddTransient<ManageStacksMenu>();
        services.AddTransient<ManageCardsMenu>();
        services.AddTransient<ReportsMenu>();
        services.AddTransient<StudySessionsMenu>();

        // register Singletons
        services.AddSingleton<ConsoleHelper>();
        services.AddSingleton<FlashCardsHelper>();

        // Register services with the container
        services.AddTransient<IStackDAO, SQLServerStackDAO>();
        services.AddTransient<ICardDAO, SQLServerCardDAO>();

        // Register the main app class (entry point for the app)
        services.AddSingleton<Program>();
    }
}
