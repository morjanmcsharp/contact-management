using CustomManagementDemo.API.DataAccessLayer;
using CustomManagementDemo.API.DataAccessLayer.Model;
using CustomManagementDemo.API.DomainLayer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CustomManagementDemo.API.Extensions.Services;

/// <summary>
/// 
/// </summary>
public static class DbServiceExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="configuration"></param>
    public static void AddMongoDb(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // Register MongoDB client
        serviceCollection.AddSingleton<IMongoClient>(s => new MongoClient(configuration.GetSection("ContactManagementDatabaseSettings").GetSection("ConnectionString").Value));

        // Register MongoDB database For the NumericIdGenerator
        serviceCollection.AddScoped<IMongoDatabase>(s =>
        {
            var client = s.GetRequiredService<IMongoClient>();
            return client.GetDatabase(configuration.GetSection("ContactManagementDatabaseSettings").GetSection("DatabaseName").Value);
        });
        
        // Register DatabaseSettings
        serviceCollection.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
        serviceCollection.Configure<DatabaseSettings>(configuration.GetSection("ContactManagementDatabaseSettings"));
        
        // Register our ApplicationDbContext
        serviceCollection.AddSingleton<ApplicationDbContext>();
     
        
    }
}