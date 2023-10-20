using CustomManagementDemo.API.DataAccessLayer.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CustomManagementDemo.API.DataAccessLayer;
/// <summary>
/// 
/// </summary>
public class ApplicationDbContext 
{
    private readonly IDatabaseSettings _settings;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="settings"></param>
    public ApplicationDbContext(IDatabaseSettings settings)
    {
        _settings = settings;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collectionName"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName)
    {
        var client = new MongoClient(_settings.ConnectionString);
        var database = client.GetDatabase(_settings.DatabaseName);
        var collection = database.GetCollection<TEntity>(collectionName);

        return collection;
    }
}