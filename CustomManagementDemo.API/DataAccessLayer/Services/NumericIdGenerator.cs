using MongoDB.Bson;
using MongoDB.Driver;

namespace CustomManagementDemo.API.DataAccessLayer.Services;

/// <summary>
/// 
/// </summary>
public class NumericIdGenerator
{
    private readonly IMongoCollection<BsonDocument> _counterCollection;

    public NumericIdGenerator(IMongoDatabase database)
    {
        _counterCollection = database.GetCollection<BsonDocument>("Counters");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collectionName"></param>
    /// <returns></returns>
    public int GenerateId(string collectionName)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", collectionName);
        var update = Builders<BsonDocument>.Update.Inc("seq", 1);
        var options = new FindOneAndUpdateOptions<BsonDocument, BsonDocument>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = true
        };

        var result = _counterCollection.FindOneAndUpdate(filter, update, options);

        return result["seq"].AsInt32;
    }
}