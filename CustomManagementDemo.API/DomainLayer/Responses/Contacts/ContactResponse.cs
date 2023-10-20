using AutoMapper;
using CustomManagementDemo.API.DomainLayer.Entities;
using CustomManagementDemo.API.DomainLayer.Entities.Contacts;
using CustomManagementDemo.API.DomainLayer.Responses.Companies;

namespace CustomManagementDemo.API.DomainLayer.Responses.Contacts;

/// <summary>
/// 
/// </summary>
public class ContactResponse
{
    /// <summary>
    /// Represent the primary key of the contact entity
    /// </summary>
    /// <example>3</example>
    public int Id { get; set; }

    /// <summary>
    /// Represent the unique name of the contact
    /// </summary>
    /// <example>Morjan</example>
    public string Name { get; set; }
    
    /// <summary>
    /// Represent a field that can be added, edited and removed dynamically
    /// </summary>
    public List<ExtendedField> ExtendedFields { get; set; } = new List<ExtendedField>();

    /// <summary>
    /// Represent a list of related companies
    /// </summary>
    public ICollection<CompanyResponse> Companies { get; set; }
}

/// <summary>
/// 
/// </summary>
public class ContactResponseProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public ContactResponseProfile()
    {
        CreateMap<Contact, ContactResponse>();
    }
}