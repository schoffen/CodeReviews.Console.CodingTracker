using Microsoft.Extensions.Configuration;

namespace schoffen.CodingTracker.Database;

public static class DatabaseConfiguration
{
    public static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        return configuration.GetConnectionString("Default");
    }
}