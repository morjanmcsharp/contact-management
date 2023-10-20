using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomManagementDemo.API.DomainLayer.Entities.Companies;

/// <summary>
/// 
/// </summary>
public class Company
{
    /// <summary>
    /// Represent the primary key of the company entity
    /// </summary>
    /// <example>5</example>
    [BsonId]
    public int Id { get; set; }

    /// <summary>
    /// Represent the unique name of the company
    /// </summary>
    /// <example>Workium</example>
    [BsonRepresentation(BsonType.String)]
    public string Name { get; set; }

    /// <summary>
    /// Represent the number of the employees in the company
    /// </summary>
    /// <example>50</example>
    [BsonRepresentation(BsonType.Int32)]
    public int EmployeesCount { get; set; }
    
    /// <summary>
    /// Represent a field that can be added, edited and removed dynamically
    /// </summary>
    public List<ExtendedField> ExtendedFields { get; set; } = new List<ExtendedField>();

    /// <summary>
    /// Represent the foreign key of the contact
    /// </summary>
    /// <example>3</example>
    [Required]
    public int ContactId { get; set; }
}