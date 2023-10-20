using AutoMapper;
using CustomManagementDemo.API.DomainLayer.Entities;
using CustomManagementDemo.API.DomainLayer.Entities.Companies;

namespace CustomManagementDemo.API.DomainLayer.Responses.Companies;

/// <summary>
/// 
/// </summary>
public class CompanyResponse
{
    /// <summary>
    /// Represent the primary key of the company entity
    /// </summary>
    /// <example>5</example>
    public int Id { get; set; }

    /// <summary>
    /// Represent the unique name of the company
    /// </summary>
    /// <example>Workium</example>
    public string Name { get; set; }

    /// <summary>
    /// Represent the number of the employees in the company
    /// </summary>
    /// <example>50</example>
    public int EmployeesCount { get; set; }
    
    /// <summary>
    /// Represent a field that can be added, edited and removed dynamically
    /// </summary>
    public List<ExtendedField> ExtendedFields { get; set; } = new List<ExtendedField>();

    /// <summary>
    /// Represent the foreign key of the contact
    /// </summary>
    /// <example>3</example>
    public int ContactId { get; set; }
}

/// <summary>
/// 
/// </summary>
public class CompanyResponseProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public CompanyResponseProfile()
    {
        CreateMap<Company, CompanyResponse>();
    }
}