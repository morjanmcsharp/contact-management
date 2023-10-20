
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomManagementDemo.API.DomainLayer.Entities.Contacts;

/// <summary>
/// 
/// </summary>
public class Contact
{
    /// <summary>
    /// Represent the primary key of the contact entity
    /// </summary>
    /// <example>3</example>
    [BsonId]
    public int Id { get; set; }

    /// <summary>
    /// Represent the unique name of the contact
    /// </summary>
    /// <example>Morjan</example>
    [BsonRepresentation(BsonType.String)]
    public string Name { get; set; }

    /// <summary>
    /// Represent a field that can be added, edited and removed dynamically
    /// </summary>
    public List<ExtendedField> ExtendedFields { get; set; } = new List<ExtendedField>();
}